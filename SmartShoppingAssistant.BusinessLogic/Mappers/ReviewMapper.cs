using SmartShoppingAssistant.BusinessLogic.DTOs.Review;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewGetDTO ToGetDTO(Review review)
        {
            return new ReviewGetDTO
            {
                Id = review.Id,
                ProductId = review.ProductId,
                UserId = review.UserId,
                CustomerName = review.User?.FullName ?? string.Empty,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
