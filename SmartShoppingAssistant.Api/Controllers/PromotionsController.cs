using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.DTOs.Promotion;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController(IPromotionService promotionService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<PromotionGetDTO>>> GetAll()
        {
            var promotions = await promotionService.GetAllAsync();
            return Ok(promotions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionGetDTO>> GetById(int id)
        {
            var promotion = await promotionService.GetByIdAsync(id);
            return Ok(promotion);
        }

        [HttpPost]
        public async Task<ActionResult<PromotionGetDTO>> Create(PromotionCreateDTO dto)
        {
            var createdPromotion = await promotionService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdPromotion.Id },
                createdPromotion
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PromotionGetDTO>> Update(int id, PromotionUpdateDTO dto)
        {
            var updatedPromotion = await promotionService.UpdateAsync(id, dto);
            return Ok(updatedPromotion);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await promotionService.DeleteAsync(id);
            return NoContent();
        }
    }
}