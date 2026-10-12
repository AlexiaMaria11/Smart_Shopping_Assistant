using SmartShoppingAssistant.BusinessLogic.DTOs.Review;
using SmartShoppingAssistant.BusinessLogic.Helpers;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class ReviewService(IReviewRepository reviewRepository) : IReviewService
{
    public async Task<List<ReviewGetDTO>> GetForProductAsync(int productId)
    {
        var reviews = await reviewRepository.GetForProductAsync(productId);
        return reviews.Select(ReviewMapper.ToGetDTO).ToList();
    }

    public async Task<ReviewEligibilityDTO> GetEligibilityAsync(int userId, int productId)
    {
        var alreadyReviewed = await reviewRepository.GetByUserAndProductAsync(userId, productId) is not null;
        var delivered = await reviewRepository.HasDeliveredItemAsync(userId, productId);

        return new ReviewEligibilityDTO
        {
            AlreadyReviewed = alreadyReviewed,
            CanReview = delivered && !alreadyReviewed
        };
    }

    public async Task<ReviewGetDTO> CreateAsync(int userId, ReviewCreateDTO dto)
    {
        if (!await reviewRepository.HasDeliveredItemAsync(userId, dto.ProductId))
            throw new BusinessException("You can review a piece only after it has been delivered to you.");

        if (await reviewRepository.GetByUserAndProductAsync(userId, dto.ProductId) is not null)
            throw new BusinessException("You have already reviewed this piece.");

        var review = new Review
        {
            ProductId = dto.ProductId,
            UserId = userId,
            Rating = dto.Rating,
            Comment = string.IsNullOrWhiteSpace(dto.Comment) ? null : dto.Comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        var created = await reviewRepository.AddAsync(review);
        // Reload with the user included so the response carries the customer's name
        var withUser = await reviewRepository.GetByUserAndProductAsync(userId, dto.ProductId);
        return ReviewMapper.ToGetDTO(withUser ?? created);
    }

    public async Task DeleteAsync(int id, CurrentUser user)
    {
        var review = await reviewRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Review with id {id} not found");

        if (review.UserId != user.Id && !user.IsAdmin)
            throw new UnauthorizedAccessException("You can only remove your own review.");

        await reviewRepository.DeleteAsync(review);
    }
}
