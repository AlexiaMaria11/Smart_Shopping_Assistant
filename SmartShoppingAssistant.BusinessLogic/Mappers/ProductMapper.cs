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
                Categories = product.Categories.Select(c => new CategoryGetDTO
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };
        }

        public static Product ToEntity(ProductCreateDTO productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description ?? string.Empty,
                ImageUrl = productDto.ImageUrl ?? string.Empty,
                Price = productDto.Price,
            };
        }

        public static void UpdateEntity(Product product, ProductUpdateDTO productDto)
        {
            product.Name = productDto.Name;
            product.Description = productDto.Description ?? string.Empty;
            product.ImageUrl = productDto.ImageUrl ?? string.Empty;
            product.Price = productDto.Price;
        }
    }
}