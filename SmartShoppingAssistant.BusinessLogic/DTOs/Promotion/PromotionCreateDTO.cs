using SmartShoppingAssistant.DataAccess.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Promotion
{
    public class PromotionCreateDTO
    {
        [Required]
        public string Name { get; set; } = null!;

        public PromotionType Type { get; set; }
        public decimal Threshold { get; set; }
        public PromotionReward Reward { get; set; }
        public int RewardValue { get; set; }

        public int? ProductId { get; set; }
        public int? CategoryId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}