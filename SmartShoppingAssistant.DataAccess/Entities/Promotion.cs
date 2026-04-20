namespace SmartShoppingAssistant.DataAccess.Entities
{
    public enum PromotionType
    {
        Percentage = 0,
        FixedAmount = 1,
        BuyXGetY = 2
    }

    public enum PromotionReward
    {
        Discount = 0,
        FreeItem = 1,
        LoyaltyPoints = 2
    }

    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public PromotionType Type { get; set; }
        public decimal Threshold { get; set; }
        public PromotionReward Reward { get; set; }
        public int RewardValue { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsActive { get; set; }
    }
}
