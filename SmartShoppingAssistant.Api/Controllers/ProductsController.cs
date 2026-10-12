using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

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

        // The Shop catalogue: filtered, sorted and paged on the server.
        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<ProductGetDTO>>> Search(
            string? name,
            [FromQuery] List<int>? categoryIds,
            decimal? minPrice,
            decimal? maxPrice,
            [FromQuery] List<int>? companyIds,
            bool? inStockOnly,
            ProductSortOption sort = ProductSortOption.PriceAsc,
            int page = 1,
            int pageSize = 24)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            return Ok(await productService.GetPagedAsync(
                categoryIds, name, minPrice, maxPrice, companyIds, inStockOnly, sort, page, pageSize));
        }

        [HttpGet("price-range")]
        public async Task<ActionResult<PriceRangeDTO>> GetPriceRange()
        {
            return Ok(await productService.GetPriceRangeAsync());
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

        // "Customers also looked at" on the product page
        [HttpGet("{id:int}/similar")]
        public async Task<ActionResult<List<ProductGetDTO>>> GetSimilar(int id, int take = 4)
        {
            return Ok(await productService.GetSimilarAsync(id, Math.Clamp(take, 1, 12)));
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
