using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController(ICartItemService cartItemService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CartGetDTO>> GetCart()
        {
            try
            {
                var cart = await cartItemService.GetAllAsync();
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
                var cart = await cartItemService.CreateAsync(dto);
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
                var cart = await cartItemService.UpdateAsync(itemId, dto);
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
                var cart = await cartItemService.DeleteAsync(itemId);
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
                await cartItemService.DeleteAllAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}