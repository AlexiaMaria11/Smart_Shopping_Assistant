using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistant.BusinessLogic.Agents;

public class PromotionCheckerAgent(IChatClient chatClient, IPromotionService promotionService) : IPromotionCheckerAgent
{
    public ChatClientAgent Build(string cartJson)
    {
        return new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = "PromotionChecker",
                Description = "Checks promotions for cart items",
                ChatOptions = new ChatOptions
                {
                    Instructions = $"""
                        You are a promotions analysis agent for an online store. Your job is to determine 
                        which promotions the current cart already qualifies for, and which ones it is 
                        close to qualifying for.

                        Current cart:
                        {cartJson}

                        Steps:
                        1. For every distinct product in the cart, call GetPromotionsForProduct to get all 
                           promotions that apply to it (either directly or via its category). Only work 
                           with promotions returned by this tool - never assume a promotion exists if the 
                           tool didn't return it.
                        2. For each promotion found, compare its rules (e.g. minimum quantity, minimum 
                           spend, category requirements) against the cart's current quantities and totals.
                        3. Classify each promotion as one of:
                           - ACTIVE: the cart already meets the promotion's requirements. Calculate the 
                             exact discount/savings the customer is currently receiving.
                           - NEAR-MISS: the cart does not yet meet the requirements, but is reasonably 
                             close (e.g. one or two more units, or a modest additional amount to reach a 
                             spending threshold). State precisely what is missing and calculate the 
                             savings the customer would get if they reached the threshold.
                           - Otherwise, ignore the promotion - it's too far from being met to be useful.
                        4. If a product has multiple applicable promotions, evaluate each one separately.
                        5. Use the same currency and number formatting as the cart data. Be precise with 
                           all calculations - do not round aggressively or estimate.
                        """,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<PromotionAnalysis>(),
                    Tools =
                    [
                        AIFunctionFactory.Create(
                            ([Description("The product ID to check")] int productId) =>
                                ShoppingTools.GetPromotionsForProduct(productId, promotionService),
                            "GetPromotionsForProduct",
                            "Get all active promotions that apply to a specific product (by product ID or its category)."
                        )
                    ]
                }
            },
            null!,
            null!
        );
    }
}