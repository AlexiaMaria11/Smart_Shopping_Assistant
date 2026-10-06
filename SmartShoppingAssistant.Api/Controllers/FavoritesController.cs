using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController(IFavoriteService favoriteService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductGetDTO>>> GetProducts()
        {
            return Ok(await favoriteService.GetProductsAsync(User.GetUserId()));
        }

        [HttpGet("ids")]
        public async Task<ActionResult<List<int>>> GetIds()
        {
            return Ok(await favoriteService.GetProductIdsAsync(User.GetUserId()));
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> Add(int productId)
        {
            await favoriteService.AddAsync(User.GetUserId(), productId);
            return NoContent();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(int productId)
        {
            await favoriteService.RemoveAsync(User.GetUserId(), productId);
            return NoContent();
        }
    }
}
