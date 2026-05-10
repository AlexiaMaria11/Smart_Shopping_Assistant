using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Promotion
{
    public class PromotionGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public PromotionType Type { get; set; }
        public decimal Threshold { get; set; }
        public PromotionReward Reward { get; set; }
        public int RewardValue { get; set; }
        public bool IsActive { get; set; }

        public int? ProductId { get; set; }

        public int? CategoryId { get; set; }
    }
}