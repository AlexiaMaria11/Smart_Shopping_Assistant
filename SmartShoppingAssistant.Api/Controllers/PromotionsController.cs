using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController(IPromotionService promotionService) : ControllerBase
    {
        private const string Managers = $"{Roles.Admin},{Roles.Seller}";

        [HttpGet]
        public async Task<ActionResult<List<PromotionGetDTO>>> GetAll(bool activeOnly)
        {
            return Ok(await promotionService.GetAllAsync(activeOnly));
        }

        [Authorize(Roles = Managers)]
        [HttpGet("manage")]
        public async Task<ActionResult<List<PromotionGetDTO>>> GetManaged()
        {
            return Ok(await promotionService.GetManagedAsync(User.ToCurrentUser()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PromotionGetDTO>> GetById(int id)
        {
            return Ok(await promotionService.GetByIdAsync(id));
        }

        // The promotions that apply to one product, shown on the product page
        [HttpGet("product/{productId:int}")]
        public async Task<ActionResult<List<PromotionGetDTO>>> GetForProduct(int productId)
        {
            return Ok(await promotionService.GetForProductAsync(productId));
        }

        [Authorize(Roles = Managers)]
        [HttpPost]
        public async Task<ActionResult<PromotionGetDTO>> Create(PromotionCreateDTO dto)
        {
            var created = await promotionService.CreateAsync(dto, User.ToCurrentUser());
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = Managers)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PromotionGetDTO>> Update(int id, PromotionUpdateDTO dto)
        {
            return Ok(await promotionService.UpdateAsync(id, dto, User.ToCurrentUser()));
        }

        [Authorize(Roles = Managers)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await promotionService.DeleteAsync(id, User.ToCurrentUser());
            return NoContent();
        }
    }
}
