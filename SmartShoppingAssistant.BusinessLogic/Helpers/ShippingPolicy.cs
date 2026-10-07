namespace SmartShoppingAssistant.BusinessLogic.Helpers;

public static class ShippingPolicy
{
    public const decimal FreeShippingThreshold = 200m;
    public const decimal StandardCost = 15.99m;

    // Based on what the customer pays for the products, after discounts
    public static decimal CostFor(decimal productsTotal) =>
        productsTotal <= 0 || productsTotal >= FreeShippingThreshold ? 0m : StandardCost;
}
