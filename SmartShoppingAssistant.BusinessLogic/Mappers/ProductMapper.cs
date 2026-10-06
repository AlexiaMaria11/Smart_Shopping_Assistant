using System.Linq;
using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class ProductMapper
    {
        public static ProductGetDTO ToGetDTO(Product product)
        {
            return new ProductGetDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description ?? string.Empty,
                ImageUrl = product.ImageUrl ?? string.Empty,
                Price = product.Price,
                CompanyId = product.CompanyId,
                CompanyName = product.Company?.Name ?? string.Empty,
                CompanySlug = product.Company?.Slug ?? string.Empty,
                Categories = product.Categories.Select(CategoryMapper.ToGetDTO).ToList()
            };
        }

        public static Product ToEntity(ProductCreateDTO productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                ImageUrl = productDto.ImageUrl,
                Price = productDto.Price,
                CompanyId = productDto.CompanyId
            };
        }

        public static void UpdateEntity(Product product, ProductUpdateDTO productDto)
        {
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.ImageUrl = productDto.ImageUrl;
            product.Price = productDto.Price;
            product.CompanyId = productDto.CompanyId;
        }
    }
}