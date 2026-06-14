using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Agents;

public class SuggestionComposerAgent(IChatClient chatClient, IProductService productService, IPromotionService promotionService) : ISuggestionComposerAgent
{
    public ChatClientAgent Build(string cartJson, string categoriesJson)
    {
        return new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = "SuggestionComposer",
                Description = "Creates smart shopping suggestions for the current cart",
                ChatOptions = new ChatOptions
                {
                    Instructions = $@"
                        You are a shopping assistant that turns the previous agent's promotion analysis 
                        into concrete, useful suggestions for the customer.

                        Current cart (includes category info):
                        {cartJson}

                        Available categories in our store:
                        {categoriesJson}

                        The previous agent's output contains:
                        - ACTIVE deals: promotions the cart already qualifies for.
                        - NEAR-MISS deals: promotions the cart is close to qualifying for, including what 
                          is missing and the potential savings.

                        Rules:
                        1. Treat NEAR-MISS deals as your primary source of suggestions. For each one, find 
                           a real product that would help the cart meet the promotion's requirement.
                           - For category-level promotions, call GetRelevantProducts with the relevant 
                             category IDs to find products from the SAME category.
                           - For product-specific promotions, you can call GetPromotionsForProduct to 
                             double check a candidate product still triggers the promotion.
                        2. Only suggest products that were actually returned by GetRelevantProducts or 
                           GetPromotionsForProduct - never invent product names, IDs or prices.
                        3. Never suggest a product that is already in the cart.
                        4. Use ACTIVE deals only as context - do not suggest anything that would reduce or 
                           invalidate a deal the customer already qualifies for.
                        5. Also suggest complementary products for items already in the cart (e.g. a phone 
                           case for a phone, a charger for a laptop), found via GetRelevantProducts.
                        6. For every suggestion, include: the product, the reason (which near-miss deal it 
                           helps complete, or what it complements), and the calculated savings (0 if it's 
                           a complementary suggestion with no direct discount).
                        7. Return at most 5 suggestions, ordered by:
                           - Suggestions tied to a near-miss deal first, sorted by savings (highest first).
                           - Then complementary product suggestions.
                        8. If there are more than 5 candidates, drop the ones with the lowest savings 
                           first - but keep at least one complementary suggestion if a good one exists.
                    ",

                    ResponseFormat = ChatResponseFormat.ForJsonSchema<AnalysisResponse>(),
                    Tools =
                    [
                        AIFunctionFactory.Create(
                            ([Description("The category IDs")] List<int> categoryIds) =>
                                ShoppingTools.GetRelevantProducts(categoryIds, productService),
                            "GetRelevantProducts",
                            "Get relevant products for categories."
                        ),
                        AIFunctionFactory.Create(
                            ([Description("The product ID to check")] int productId) =>
                                ShoppingTools.GetPromotionsForProduct(productId, promotionService),
                            "GetPromotionsForProduct",
                            "Get all active promotions for a product."
                        )
                    ]
                }
            },
            null!,
            null!
        );
    }
}