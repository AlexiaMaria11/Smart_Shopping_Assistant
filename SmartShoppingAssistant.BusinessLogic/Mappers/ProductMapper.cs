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
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                CategoryIds = product.Categories?.Select(c => c.Id).ToList() ?? new List<int>() { 0 },
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
            };
        }

        public static void UpdateEntity(Product product, ProductUpdateDTO productDto)
        {
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.ImageUrl = productDto.ImageUrl;
            product.Price = productDto.Price;
        }
    }
}