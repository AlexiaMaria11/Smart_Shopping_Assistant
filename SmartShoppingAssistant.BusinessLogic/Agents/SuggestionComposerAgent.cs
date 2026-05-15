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
                        You create shopping suggestions based on the promotion analysis from the
                        previous agent and the current cart (which includes category info):
                        {cartJson}

                        Available categories in our store:
                        {categoriesJson}

                        Rules:
                        1. The previous agent's output contains active deals (already qualifying) and
                           near-miss deals (almost qualifying). Use both to generate suggestions.
                        2. For category-level promotions, suggest items from the SAME category that
                           would help trigger the deal (e.g. adding another Electronics item to meet
                           a category quantity threshold).
                        3. Use GetRelevantProducts/GetPromotionsForProduct to find real products — only
                           suggest products that the tools actually returned.
                        4. Include calculated savings for each suggestion.
                        5. Also suggest complementary products based on what's in the cart
                           (e.g. phone case for a phone, charger for a laptop).
                        6. Max 5 suggestions, prioritizing those with the highest savings (do not 
                           include the suggestions with no savings).",

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