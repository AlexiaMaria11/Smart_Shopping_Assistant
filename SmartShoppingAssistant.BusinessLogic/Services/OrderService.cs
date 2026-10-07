using SmartShoppingAssistant.BusinessLogic.DTOs.Order;
using SmartShoppingAssistant.BusinessLogic.Helpers;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class OrderService(
    IOrderRepository orderRepository,
    ICartItemRepository cartItemRepository,
    ICartService cartService) : IOrderService
{
    public async Task<OrderGetDTO> PlaceOrderAsync(int userId, CheckoutDTO dto)
    {
        var cartItems = await cartItemRepository.GetForUserAsync(userId);
        if (cartItems.Count == 0)
            throw new BusinessException("Your cart is empty.");

        var unavailable = cartItems.FirstOrDefault(i => i.Product.Company.Status != CompanyStatus.Approved);
        if (unavailable is not null)
            throw new BusinessException($"{unavailable.Product.Name} is no longer sold. Please remove it from your cart.");

        string? cardLast4 = dto.PaymentMethod == PaymentMethod.Card
            ? CardValidator.Validate(dto.Card, DateTime.UtcNow)
            : null;

        // Same prices and promotions the customer saw in the cart
        var cart = await cartService.GetCartAsync(userId);
        var productsTotal = cart.Total;
        var shippingCost = ShippingPolicy.CostFor(productsTotal);

        await using var transaction = await orderRepository.BeginTransactionAsync();

        foreach (var item in cartItems)
        {
            if (!await orderRepository.TryReserveStockAsync(item.ProductId, item.Quantity))
                throw new BusinessException(
                    $"There is not enough {item.Product.Name} in stock anymore. Please update your cart.");
        }

        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            PaymentMethod = dto.PaymentMethod,
            IsPaid = dto.PaymentMethod == PaymentMethod.Card,
            CardLast4 = cardLast4,
            ShippingFullName = dto.ShippingFullName.Trim(),
            ShippingPhone = dto.ShippingPhone.Trim(),
            ShippingAddress = dto.ShippingAddress.Trim(),
            ShippingCity = dto.ShippingCity.Trim(),
            ShippingCounty = dto.ShippingCounty.Trim(),
            ShippingPostalCode = dto.ShippingPostalCode.Trim(),
            Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(),
            Subtotal = cart.Subtotal,
            Discount = -cart.TotalDiscount,
            ShippingCost = shippingCost,
            Total = productsTotal + shippingCost,
            AppliedPromotions = cart.AppliedPromotions
                .Select(p => new OrderPromotion { PromotionId = p.PromotionId, Name = p.PromotionName, Discount = -p.Discount })
                .ToList(),
            Items = cartItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ImageUrl = i.Product.ImageUrl,
                UnitPrice = i.Product.Price,
                Quantity = i.Quantity,
                LineTotal = i.Product.Price * i.Quantity,
                CompanyId = i.Product.CompanyId,
                Status = OrderStatus.Pending
            }).ToList()
        };

        await orderRepository.AddAsync(order);
        await cartItemRepository.ClearAsync(userId);
        await transaction.CommitAsync();

        return await GetAsync(order.Id, new CurrentUser(userId, Roles.Customer, null));
    }

    public async Task<List<OrderGetDTO>> GetMineAsync(int userId)
    {
        var orders = await orderRepository.GetForUserAsync(userId);
        return orders.Select(OrderMapper.ToGetDTO).ToList();
    }

    public async Task<List<OrderGetDTO>> GetForSellerAsync(CurrentUser user)
    {
        var companyId = user.RequireCompanyId();
        var orders = await orderRepository.GetForCompanyAsync(companyId);
        return orders.Select(o => OrderMapper.ToSellerDTO(o, companyId)).ToList();
    }

    public async Task<List<OrderGetDTO>> GetAllAsync()
    {
        var orders = await orderRepository.GetAllWithItemsAsync();
        return orders.Select(OrderMapper.ToGetDTO).ToList();
    }

    public async Task<OrderGetDTO> GetAsync(int id, CurrentUser user)
    {
        var order = await orderRepository.GetWithItemsAsync(id);

        if (user.IsAdmin || order.UserId == user.Id)
            return OrderMapper.ToGetDTO(order);

        if (user.IsSeller && user.CompanyId.HasValue && order.Items.Any(i => i.CompanyId == user.CompanyId))
            return OrderMapper.ToSellerDTO(order, user.CompanyId.Value);

        // Do not reveal that the order exists
        throw new KeyNotFoundException($"Order with id {id} not found");
    }

    public async Task<OrderGetDTO> CancelAsync(int id, int userId)
    {
        var order = await orderRepository.GetWithItemsAsync(id);
        if (order.UserId != userId)
            throw new KeyNotFoundException($"Order with id {id} not found");

        if (!OrderMapper.CanCancel(order))
            throw new BusinessException("This order can no longer be cancelled because part of it has already been shipped.");

        await using var transaction = await orderRepository.BeginTransactionAsync();
        foreach (var item in order.Items.Where(i => i.Status != OrderStatus.Cancelled))
            await CancelItemAsync(item);

        RefreshOrderStatus(order);
        await orderRepository.SaveChangesAsync();
        await transaction.CommitAsync();

        return OrderMapper.ToGetDTO(order);
    }

    // A seller moves their own package forward; an admin can do it for any seller
    public async Task<OrderGetDTO> UpdateSellerStatusAsync(int id, int companyId, OrderStatus status, CurrentUser user)
    {
        user.EnsureCanManageCompany(companyId);

        var order = await orderRepository.GetWithItemsAsync(id);
        var items = order.Items.Where(i => i.CompanyId == companyId && i.Status != OrderStatus.Cancelled).ToList();
        if (items.Count == 0)
            throw new KeyNotFoundException($"Order with id {id} not found");

        var current = items.Min(i => i.Status);
        if (!IsAllowedTransition(current, status))
            throw new BusinessException($"An order that is {current} cannot be marked as {status}.");

        await using var transaction = await orderRepository.BeginTransactionAsync();
        foreach (var item in items)
        {
            if (status == OrderStatus.Cancelled)
                await CancelItemAsync(item);
            else
                item.Status = status;
        }

        RefreshOrderStatus(order);
        await orderRepository.SaveChangesAsync();
        await transaction.CommitAsync();

        return user.IsAdmin ? OrderMapper.ToGetDTO(order) : OrderMapper.ToSellerDTO(order, companyId);
    }

    private static bool IsAllowedTransition(OrderStatus from, OrderStatus to) => (from, to) switch
    {
        (OrderStatus.Pending, OrderStatus.Confirmed) => true,
        (OrderStatus.Confirmed, OrderStatus.Shipped) => true,
        (OrderStatus.Shipped, OrderStatus.Delivered) => true,
        (OrderStatus.Pending or OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
        _ => false
    };

    private async Task CancelItemAsync(OrderItem item)
    {
        item.Status = OrderStatus.Cancelled;
        if (item.ProductId.HasValue)
            await orderRepository.RestoreStockAsync(item.ProductId.Value, item.Quantity);
    }

    // The order is as far along as its slowest package; cash is collected when everything is delivered
    private static void RefreshOrderStatus(Order order)
    {
        var active = order.Items.Where(i => i.Status != OrderStatus.Cancelled).ToList();
        order.Status = active.Count == 0 ? OrderStatus.Cancelled : active.Min(i => i.Status);

        if (order.Status == OrderStatus.Delivered && order.PaymentMethod == PaymentMethod.CashOnDelivery)
            order.IsPaid = true;
    }
}
