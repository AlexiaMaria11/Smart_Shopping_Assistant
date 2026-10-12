namespace SmartShoppingAssistant.DataAccess.Repositories;

public record OrderStatsSummary(
    decimal RevenueToday,
    decimal Revenue30Days,
    decimal RevenueTotal,
    int OrdersToday,
    int Orders30Days,
    int OrdersTotal);

public record ProductSalesSummary(int? ProductId, string ProductName, int QuantitySold, decimal Revenue);

public record CategorySalesSummary(int CategoryId, string CategoryName, int QuantitySold, decimal Revenue);
