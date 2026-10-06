namespace SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;

public class AppliedPromotionDTO
{
    public int PromotionId { get; set; }
    public string PromotionName { get; set; } = null!;
    public decimal Discount { get; set; }
}