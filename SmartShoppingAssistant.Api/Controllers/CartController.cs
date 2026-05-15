using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CartGetDTO>> GetCart()
        {
            try
            {
                var cart = await cartService.GetCartAsync();
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartGetDTO>> AddItem(CartItemCreateDTO dto)
        {
            try
            {
                var cart = await cartService.AddItemAsync(dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("items/{itemId}")]
        public async Task<ActionResult<CartGetDTO>> UpdateItem(
            int itemId,
            CartItemUpdateDTO dto)
        {
            try
            {
                var cart = await cartService.UpdateItemAsync(itemId, dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("items/{itemId}")]
        public async Task<ActionResult<CartGetDTO>> RemoveItem(int itemId)
        {
            try
            {
                var cart = await cartService.RemoveItemAsync(itemId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                await cartService.ClearCartAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeCart()
        {
            var analysisResponse = await cartService.AnalyzeCartAsync();
            return Ok(analysisResponse);
        }
    }
}