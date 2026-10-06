using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private const string Managers = $"{Roles.Admin},{Roles.Seller}";

        [HttpGet]
        public async Task<ActionResult<List<ProductGetDTO>>> GetAll(
             string? name,
             int? categoryId,
             decimal? minPrice,
             decimal? maxPrice,
             int? companyId)
        {
            return Ok(await productService.GetAllAsync(categoryId, name, minPrice, maxPrice, companyId));
        }

        [Authorize(Roles = Managers)]
        [HttpGet("manage")]
        public async Task<ActionResult<List<ProductGetDTO>>> GetManaged()
        {
            return Ok(await productService.GetManagedAsync(User.ToCurrentUser()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductGetDTO>> GetById(int id)
        {
            return Ok(await productService.GetByIdAsync(id));
        }

        [Authorize(Roles = Managers)]
        [HttpPost]
        public async Task<ActionResult<ProductGetDTO>> Create(ProductCreateDTO dto)
        {
            var created = await productService.CreateAsync(dto, User.ToCurrentUser());
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = Managers)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductGetDTO>> Update(int id, ProductUpdateDTO dto)
        {
            return Ok(await productService.UpdateAsync(id, dto, User.ToCurrentUser()));
        }

        [Authorize(Roles = Managers)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await productService.DeleteAsync(id, User.ToCurrentUser());
            return NoContent();
        }
    }
}
