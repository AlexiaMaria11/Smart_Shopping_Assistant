namespace SmartShoppingAssistant.BusinessLogic.DTOs.Review
{
    public class ReviewEligibilityDTO
    {
        // The piece was delivered to this customer and they have not reviewed it yet
        public bool CanReview { get; set; }
        public bool AlreadyReviewed { get; set; }
    }
}
