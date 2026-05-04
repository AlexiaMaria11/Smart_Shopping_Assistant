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
            try
            {
                var promotions = await promotionService.GetAllAsync();
                return Ok(promotions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionGetDTO>> GetById(int id)
        {
            try
            {
                var promotion = await promotionService.GetByIdAsync(id);
                return Ok(promotion);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PromotionGetDTO>> Create(PromotionCreateDTO dto)
        {
            try
            {
                var createdPromotion = await promotionService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdPromotion.Id },
                    createdPromotion
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PromotionGetDTO>> Update(int id, PromotionUpdateDTO dto)
        {
            try
            {
                var updatedPromotion = await promotionService.UpdateAsync(id, dto);
                return Ok(updatedPromotion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await promotionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}