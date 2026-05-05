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
                Product = ToProductGetDTO(ci.Product)
            };
        }

        public static ProductGetDTO ToProductGetDTO(Product product)
        {
            return new ProductGetDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description ?? string.Empty,
                ImageUrl = product.ImageUrl ?? string.Empty,
                Price = product.Price,
                Categories = product.Categories?
                    .Select(ToCategoryGetDTO)
                    .ToList() ?? new List<CategoryGetDTO>()
            };
        }

        public static CategoryGetDTO ToCategoryGetDTO(Category category)
        {
            return new CategoryGetDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description ?? string.Empty
            };
        }

        public static CartGetDTO ToCartGetDTO(
            List<CartItemGetDTO> items,
            decimal cartTotal,
            decimal discount,
            decimal finalTotal)
        {
            return new CartGetDTO
            {
                Items = items,
                CartTotal = decimal.Round(cartTotal, 2),
                Discount = decimal.Round(discount, 2),
                FinalTotal = decimal.Round(finalTotal, 2)
            };
        }
    }
}