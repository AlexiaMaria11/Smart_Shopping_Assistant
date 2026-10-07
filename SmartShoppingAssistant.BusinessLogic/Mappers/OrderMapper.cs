using SmartShoppingAssistant.BusinessLogic.DTOs.Order;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class OrderMapper
    {
        public static string Number(int orderId) => $"SSA-{orderId:D6}";

        // The customer can cancel until any package has left the warehouse
        public static bool CanCancel(Order order) =>
            order.Status != OrderStatus.Cancelled &&
            order.Items.All(i => i.Status is OrderStatus.Pending or OrderStatus.Confirmed or OrderStatus.Cancelled);

        public static OrderGetDTO ToGetDTO(Order order)
        {
            return new OrderGetDTO
            {
                Id = order.Id,
                Number = Number(order.Id),
                CreatedAt = DateTime.SpecifyKind(order.CreatedAt, DateTimeKind.Utc),
                Status = order.Status,
                CanCancel = CanCancel(order),
                CustomerName = order.User?.FullName ?? string.Empty,
                CustomerEmail = order.User?.Email ?? string.Empty,
                PaymentMethod = order.PaymentMethod,
                IsPaid = order.IsPaid,
                CardLast4 = order.CardLast4,
                ShippingFullName = order.ShippingFullName,
                ShippingPhone = order.ShippingPhone,
                ShippingAddress = order.ShippingAddress,
                ShippingCity = order.ShippingCity,
                ShippingCounty = order.ShippingCounty,
                ShippingPostalCode = order.ShippingPostalCode,
                Notes = order.Notes,
                Subtotal = order.Subtotal,
                Discount = order.Discount,
                ShippingCost = order.ShippingCost,
                Total = order.Total,
                AppliedPromotions = order.AppliedPromotions
                    .Select(p => new OrderPromotionDTO { Name = p.Name, Discount = p.Discount })
                    .ToList(),
                Items = order.Items.OrderBy(i => i.Id).Select(ToItemDTO).ToList()
            };
        }

        // A seller only sees their own package: their items, their status and their part of the money
        public static OrderGetDTO ToSellerDTO(Order order, int companyId)
        {
            var dto = ToGetDTO(order);
            dto.Items = dto.Items.Where(i => i.CompanyId == companyId).ToList();

            var active = dto.Items.Where(i => i.Status != OrderStatus.Cancelled).ToList();
            dto.Status = active.Count == 0 ? OrderStatus.Cancelled : active.Min(i => i.Status);
            dto.Subtotal = dto.Items.Where(i => i.Status != OrderStatus.Cancelled).Sum(i => i.LineTotal);
            dto.Total = dto.Subtotal;
            dto.Discount = 0;
            dto.ShippingCost = 0;
            dto.AppliedPromotions = [];
            dto.CanCancel = false;
            return dto;
        }

        private static OrderItemGetDTO ToItemDTO(OrderItem item)
        {
            return new OrderItemGetDTO
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ImageUrl = item.ImageUrl,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                LineTotal = item.LineTotal,
                CompanyId = item.CompanyId,
                CompanyName = item.Company?.Name ?? string.Empty,
                Status = item.Status
            };
        }
    }
}
