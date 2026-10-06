using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.Company;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController(ICompanyService companyService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CompanyGetDTO>>> GetAll()
        {
            return Ok(await companyService.GetAllAsync(approvedOnly: true));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("all")]
        public async Task<ActionResult<List<CompanyGetDTO>>> GetAllIncludingPending()
        {
            return Ok(await companyService.GetAllAsync(approvedOnly: false));
        }

        [Authorize(Roles = Roles.Seller)]
        [HttpGet("mine")]
        public async Task<ActionResult<CompanyGetDTO>> GetMine()
        {
            return Ok(await companyService.GetByIdAsync(User.ToCurrentUser().RequireCompanyId()));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompanyGetDTO>> GetById(int id)
        {
            return Ok(await companyService.GetByIdAsync(id));
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<CompanyGetDTO>> GetBySlug(string slug)
        {
            return Ok(await companyService.GetBySlugAsync(slug));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<CompanyGetDTO>> Create(CompanyCreateDTO dto)
        {
            var created = await companyService.CreateAsync(dto, CompanyStatus.Approved);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.Seller}")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CompanyGetDTO>> Update(int id, CompanyUpdateDTO dto)
        {
            return Ok(await companyService.UpdateAsync(id, dto, User.ToCurrentUser()));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id:int}/status")]
        public async Task<ActionResult<CompanyGetDTO>> UpdateStatus(int id, CompanyStatusUpdateDTO dto)
        {
            return Ok(await companyService.UpdateStatusAsync(id, dto));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await companyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
