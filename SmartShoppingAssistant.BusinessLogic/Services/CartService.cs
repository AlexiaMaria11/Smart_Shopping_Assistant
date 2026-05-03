using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class CartService(
        IRepository<CartItem> cartItemRepository,
        IRepository<Product> productRepository
    ) : ICartService
    {
        public async Task<CartGetDTO> GetCartAsync()
        {
            var cartItems = await cartItemRepository.GetAllAsync();
            return await ToCartGetDTOAsync(cartItems.ToList());
        }

        public async Task<CartGetDTO> AddItemAsync(CartItemCreateDTO dto)
        {
            var product = await productRepository.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found.");

            var cartItems = await cartItemRepository.GetAllAsync();

            var existingItem = cartItems
                .FirstOrDefault(ci => ci.ProductId == dto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                await cartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };

                await cartItemRepository.AddAsync(cartItem);
            }

            return await GetCartAsync();
        }

        public async Task<CartGetDTO> UpdateItemQuantityAsync(int itemId, CartItemUpdateDTO dto)
        {
            var cartItem = await cartItemRepository.GetByIdAsync(itemId);

            if (cartItem == null)
                throw new Exception("Cart item not found.");

            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0.");

            cartItem.Quantity = dto.Quantity;

            await cartItemRepository.UpdateAsync(cartItem);

            return await GetCartAsync();
        }

        public async Task<CartGetDTO> RemoveItemAsync(int itemId)
        {
            var cartItem = await cartItemRepository.GetByIdAsync(itemId);

            if (cartItem == null)
                throw new Exception("Cart item not found.");

            await cartItemRepository.DeleteAsync(itemId);

            return await GetCartAsync();
        }

        public async Task ClearCartAsync()
        {
            var cartItems = await cartItemRepository.GetAllAsync();

            foreach (var item in cartItems)
            {
                await cartItemRepository.DeleteAsync(item.Id);
            }
        }

        private async Task<CartGetDTO> ToCartGetDTOAsync(List<CartItem> cartItems)
        {
            var items = new List<CartItemGetDTO>();

            foreach (var ci in cartItems)
            {
                var product = await productRepository.GetByIdAsync(ci.ProductId);

                items.Add(new CartItemGetDTO
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = ci.Quantity,
                    TotalPrice = product.Price * ci.Quantity
                });
            }

            return new CartGetDTO
            {
                Items = items,
                Total = items.Sum(i => i.TotalPrice)
            };
        }
    }
}