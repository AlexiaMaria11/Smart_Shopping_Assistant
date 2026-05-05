using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class CartMapper
    {
        public static CartItemGetDTO ToItemGetDTO(CartItem ci)
        {
            return new CartItemGetDTO
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Product = new ProductGetDTO
                {
                    Id = ci.Product.Id,
                    Name = ci.Product.Name,
                    Description = ci.Product.Description ?? string.Empty,
                    ImageUrl = ci.Product.ImageUrl ?? string.Empty,
                    Price = ci.Product.Price,
                    Categories = ci.Product.Categories?
                        .Select(c => new CategoryGetDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description ?? string.Empty
                        })
                        .ToList() ?? new List<CategoryGetDTO>()
                }
            };
        }
    }
}