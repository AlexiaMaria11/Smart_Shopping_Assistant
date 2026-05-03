using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/cart")]
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
        public async Task<ActionResult<CartGetDTO>> AddItem([FromBody] CartItemCreateDTO dto)
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
        public async Task<ActionResult<CartGetDTO>> UpdateItemQuantity(
            int itemId,
            [FromBody] CartItemUpdateDTO dto)
        {
            try
            {
                var cart = await cartService.UpdateItemQuantityAsync(itemId, dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest(ex.Message);
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
    }
}