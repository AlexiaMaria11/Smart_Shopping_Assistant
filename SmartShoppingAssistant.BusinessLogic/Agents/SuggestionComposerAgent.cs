using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Agents;

public class SuggestionComposerAgent(IChatClient chatClient, IProductService productService, IPromotionService promotionService) : ISuggestionComposerAgent
{
    public ChatClientAgent Build(string cartJson, string categoriesJson, string promotionAnalysisJson)
    {
        return new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = "SuggestionComposer",
                Description = "Creates smart shopping suggestions for the current cart",
                ChatOptions = new ChatOptions
                {
                    Instructions = $"""
                        You are a shopping suggestion assistant.
                        CURRENT CART:
                        {cartJson}
                        AVAILABLE CATEGORIES:
                        {categoriesJson}
                        PROMOTION ANALYSIS:
                        {promotionAnalysisJson}
                        TASKS:
                        1. Analyze the products already present in the cart.
                        2. Find complementary or relevant products.
                        3. Prioritize products that help activate near-miss promotions.
                        4. Use available categories when generating suggestions.
                        5. Return a maximum of 5 suggestions.
                        6. Avoid duplicates.
                        IMPORTANT:
                        - Return ONLY valid JSON.
                        - Do not include explanations outside JSON.
                        - Prefer products related to promotions when possible.
                        """,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<SuggestionAnalysis>(),
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