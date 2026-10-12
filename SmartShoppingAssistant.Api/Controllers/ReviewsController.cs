using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.Review;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(IReviewService reviewService) : ControllerBase
    {
        [HttpGet("product/{productId:int}")]
        public async Task<ActionResult<List<ReviewGetDTO>>> GetForProduct(int productId)
        {
            return Ok(await reviewService.GetForProductAsync(productId));
        }

        [Authorize]
        [HttpGet("product/{productId:int}/eligibility")]
        public async Task<ActionResult<ReviewEligibilityDTO>> GetEligibility(int productId)
        {
            return Ok(await reviewService.GetEligibilityAsync(User.GetUserId(), productId));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReviewGetDTO>> Create(ReviewCreateDTO dto)
        {
            var created = await reviewService.CreateAsync(User.GetUserId(), dto);
            return CreatedAtAction(nameof(GetForProduct), new { productId = created.ProductId }, created);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await reviewService.DeleteAsync(id, User.ToCurrentUser());
            return NoContent();
        }
    }
}
