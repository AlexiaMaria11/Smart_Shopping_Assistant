using SmartShoppingAssistant.BusinessLogic.Helpers;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BusinessLogic.Agents;
using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repositories;
using System.Text.Json;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CartService(ICartItemRepository cartItemRepository, IProductRepository productRepository, IPromotionRepository promotionRepository, ICategoryRepository categoryRepository, IPromotionCheckerAgent promotionCheckerAgent, ISuggestionComposerAgent suggestionComposerAgent) : ICartService
{
    public async Task<CartGetDTO> GetCartAsync(int userId)
    {
        var cartItems = await cartItemRepository.GetForUserAsync(userId);
        var promotions = (await promotionRepository.GetAllAsync()).Where(p => p.IsActive).ToList();

        var subtotal = cartItems.Sum(i => i.Product.Price * i.Quantity);

        var appliedPromotions = promotions
            .Select(p => (Promotion: p, Discount: CalculateDiscount(p, cartItems, subtotal)))
            .Where(x => x.Discount > 0)
            .Select(x => new AppliedPromotionDTO { PromotionId = x.Promotion.Id, PromotionName = x.Promotion.Name, Discount = -x.Discount })
            .ToList();

        var totalDiscount = Math.Max(appliedPromotions.Sum(x => x.Discount), -subtotal);

        return new CartGetDTO
        {
            Items = cartItems.Select(CartMapper.ToItemGetDTO).ToList(),
            Subtotal = subtotal,
            AppliedPromotions = appliedPromotions,
            TotalDiscount = totalDiscount,
            Total = subtotal + totalDiscount
        };
    }

    public async Task<CartGetDTO> AddItemAsync(int userId, CartItemCreateDTO dto)
    {
        if (dto.Quantity < 1)
            throw new ArgumentException("Quantity must be at least 1.");

        await productRepository.GetByIdAsync(dto.ProductId);

        var existing = await cartItemRepository.GetByProductIdAsync(userId, dto.ProductId);

        if (existing != null)
        {
            existing.Quantity += dto.Quantity;
            await cartItemRepository.UpdateAsync(existing);
        }
        else
        {
            var item = new CartItem
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await cartItemRepository.AddAsync(item);
        }

        return await GetCartAsync(userId);
    }

    public async Task<CartGetDTO> UpdateItemAsync(int userId, int itemId, CartItemUpdateDTO dto)
    {
        if (dto.Quantity < 1)
            throw new ArgumentException("Quantity must be at least 1.");

        var item = await cartItemRepository.GetForUserByIdAsync(userId, itemId);

        item.Quantity = dto.Quantity;

        await cartItemRepository.UpdateAsync(item);

        return await GetCartAsync(userId);
    }

    public async Task<CartGetDTO> RemoveItemAsync(int userId, int itemId)
    {
        await cartItemRepository.GetForUserByIdAsync(userId, itemId);
        await cartItemRepository.DeleteAsync(itemId);
        return await GetCartAsync(userId);
    }

    public Task ClearCartAsync(int userId) => cartItemRepository.ClearAsync(userId);

    private static decimal CalculateDiscount(Promotion promo, List<CartItem> cartItems, decimal cartTotal)
    {
        // A company promotion only ever applies to that company's own products
        var eligible = promo.CompanyId.HasValue
            ? cartItems.Where(i => i.Product.CompanyId == promo.CompanyId.Value).ToList()
            : cartItems;

        List<CartItem> applicable;
        if (promo.ProductId.HasValue)
        {
            var item = eligible.FirstOrDefault(i => i.ProductId == promo.ProductId.Value);
            applicable = item is null ? [] : [item];
        }
        else if (promo.CategoryId.HasValue)
        {
            applicable = eligible
                .Where(i => i.Product.Categories.Any(c => c.Id == promo.CategoryId.Value))
                .ToList();
        }
        else
        {
            applicable = eligible;
        }

        if (applicable.Count == 0) return 0;

        var applicableTotal = applicable.Sum(i => i.Product.Price * i.Quantity);
        var applicableQuantity = applicable.Sum(i => i.Quantity);

        var triggered = promo.Type switch
        {
            PromotionType.Quantity => applicableQuantity >= promo.Threshold,
            PromotionType.CartTotal => applicableTotal >= promo.Threshold,
            _ => false
        };

        if (!triggered) return 0;

        return promo.Reward switch
        {
            PromotionReward.PercentDiscount => applicableTotal * promo.RewardValue / 100m,
            PromotionReward.FreeItems when promo.ProductId.HasValue =>
                Math.Min(promo.RewardValue, applicable[0].Quantity) * applicable[0].Product.Price,
            PromotionReward.FreeItems =>
                applicable
                    .SelectMany(i => Enumerable.Repeat(i.Product.Price, i.Quantity))
                    .OrderBy(p => p)
                    .Take(promo.RewardValue)
                    .Sum(),
            _ => 0
        };
    }
    public async Task<AnalysisResponse> AnalyzeCartAsync(int userId)
    {
        var cart = await cartItemRepository.GetForUserAsync(userId);
        if (cart.Count == 0)
            throw new BusinessException("Your cart is empty. Add some products before running the analysis.");

        var categories = await categoryRepository.GetAllAsync();

        var cartJson = JsonSerializer.Serialize(cart.Select(c => new
        {
            c.ProductId,
            c.Product.Price,
            c.Quantity,
            LineTotal = c.Product.Price*c.Quantity,
            Seller = c.Product.Company.Name,
            CategoryIds = c.Product.Categories.Select(cat => new {CategoryId = cat.Id, CategoryName = cat.Name}).ToList(),
        }));

        var categoryJson = JsonSerializer.Serialize(categories.Select(c => new
        {
            Categoryid = c.Id,
            CategoryName = c.Name,
        }));

        var promotionAgent = promotionCheckerAgent.Build(cartJson);
        var suggestionAgent = suggestionComposerAgent.Build(cartJson, categoryJson);

        var workflow = new WorkflowBuilder(promotionAgent).AddEdge(promotionAgent, suggestionAgent)
            .WithOutputFrom(suggestionAgent)
            .Build();

        var chatMessage = new List<ChatMessage>
        {
            new(ChatRole.User, "Analyze the current cart and suggest improvements.")
        };

        try
        {
            return await RunWorkflowAsync(workflow, chatMessage);
        }
        catch (Exception ex) when (ex is not BusinessException)
        {
            throw new BusinessException("The AI assistant is not available right now. Please try again later.", ex);
        }
    }

    private static async Task<AnalysisResponse> RunWorkflowAsync(Workflow workflow, List<ChatMessage> chatMessage)
    {
        await using var result = await InProcessExecution.RunStreamingAsync(workflow, chatMessage);

        await result.TrySendMessageAsync(new TurnToken(emitEvents:true));

        var jsonBuilder = new System.Text.StringBuilder();

        await foreach(var message in result.WatchStreamAsync())
        {
            if(message is AgentResponseUpdateEvent update && update.ExecutorId.StartsWith("SuggestionComposer"))
            {
                jsonBuilder.Append(update.Update.Text);
            }
            else if(message is WorkflowErrorEvent errorEvent)
            {
                throw new InvalidOperationException(errorEvent.Exception?.Message);
            }
        }

        var json = jsonBuilder.ToString();
        return JsonSerializer.Deserialize<AnalysisResponse>(json) ?? throw new InvalidOperationException("Failed to deserialize analysis response");
    }
}