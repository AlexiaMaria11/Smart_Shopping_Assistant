namespace SmartShoppingAssistant.BusinessLogic.DTOs.Stats;

public record StatsPeriodDTO(decimal Revenue, int OrderCount);

public class TopProductDTO
{
    public int? ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class TopCategoryDTO
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class SellerStatsDTO
{
    public StatsPeriodDTO Today { get; set; } = null!;
    public StatsPeriodDTO Last30Days { get; set; } = null!;
    public StatsPeriodDTO AllTime { get; set; } = null!;
    public List<TopProductDTO> TopProducts { get; set; } = [];
}

public class AdminStatsDTO
{
    public StatsPeriodDTO Today { get; set; } = null!;
    public StatsPeriodDTO Last30Days { get; set; } = null!;
    public StatsPeriodDTO AllTime { get; set; } = null!;
    public List<TopProductDTO> TopProducts { get; set; } = [];
    public List<TopCategoryDTO> TopCategories { get; set; } = [];
}
