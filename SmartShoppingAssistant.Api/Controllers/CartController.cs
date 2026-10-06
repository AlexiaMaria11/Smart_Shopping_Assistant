using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CartGetDTO>> GetCart()
        {
            return Ok(await cartService.GetCartAsync(User.GetUserId()));
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartGetDTO>> AddItem(CartItemCreateDTO dto)
        {
            return Ok(await cartService.AddItemAsync(User.GetUserId(), dto));
        }

        [HttpPut("items/{itemId}")]
        public async Task<ActionResult<CartGetDTO>> UpdateItem(int itemId, CartItemUpdateDTO dto)
        {
            return Ok(await cartService.UpdateItemAsync(User.GetUserId(), itemId, dto));
        }

        [HttpDelete("items/{itemId}")]
        public async Task<ActionResult<CartGetDTO>> RemoveItem(int itemId)
        {
            return Ok(await cartService.RemoveItemAsync(User.GetUserId(), itemId));
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await cartService.ClearCartAsync(User.GetUserId());
            return NoContent();
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<AnalysisResponse>> AnalyzeCart()
        {
            return Ok(await cartService.AnalyzeCartAsync(User.GetUserId()));
        }
    }
}
