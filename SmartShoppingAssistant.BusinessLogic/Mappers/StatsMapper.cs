using SmartShoppingAssistant.BusinessLogic.DTOs.Stats;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class StatsMapper
    {
        public static SellerStatsDTO ToSellerDTO(OrderStatsSummary summary, List<ProductSalesSummary> topProducts) => new()
        {
            Today = new StatsPeriodDTO(summary.RevenueToday, summary.OrdersToday),
            Last30Days = new StatsPeriodDTO(summary.Revenue30Days, summary.Orders30Days),
            AllTime = new StatsPeriodDTO(summary.RevenueTotal, summary.OrdersTotal),
            TopProducts = topProducts.Select(ToTopProductDTO).ToList(),
        };

        public static AdminStatsDTO ToAdminDTO(
            OrderStatsSummary summary,
            List<ProductSalesSummary> topProducts,
            List<CategorySalesSummary> topCategories) => new()
        {
            Today = new StatsPeriodDTO(summary.RevenueToday, summary.OrdersToday),
            Last30Days = new StatsPeriodDTO(summary.Revenue30Days, summary.Orders30Days),
            AllTime = new StatsPeriodDTO(summary.RevenueTotal, summary.OrdersTotal),
            TopProducts = topProducts.Select(ToTopProductDTO).ToList(),
            TopCategories = topCategories.Select(c => new TopCategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                QuantitySold = c.QuantitySold,
                Revenue = c.Revenue,
            }).ToList(),
        };

        private static TopProductDTO ToTopProductDTO(ProductSalesSummary p) => new()
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            QuantitySold = p.QuantitySold,
            Revenue = p.Revenue,
        };
    }
}
