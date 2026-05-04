using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.BusinessLogic.DTOs.Cart;
using SmartShoppingAssistant.BusinessLogic.DTOs.Product;
using SmartShoppingAssistant.BusinessLogic.DTOs.Category;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services
{
    public class CartItemService(
        IRepository<CartItem> cartItemRepository,
        IProductRepository productRepository,
        IPromotionRepository promotionRepository
    ) : ICartItemService
    {
        public async Task<CartGetDTO> GetAllAsync()
        {
            var cartItems = await cartItemRepository.GetAllAsync();

            return await BuildCartDTO(cartItems.ToList());
        }

        public async Task<CartItemGetDTO> GetByIdAsync(int id)
        {
            var item = await cartItemRepository.GetByIdAsync(id);

            item.Product = await productRepository.GetByIdAsyncWithCategories(item.ProductId);

            return new CartItemGetDTO
            {
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Product = new ProductGetDTO
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name,
                    Description = item.Product.Description ?? string.Empty,
                    ImageUrl = item.Product.ImageUrl ?? string.Empty,
                    Price = item.Product.Price,
                    Categories = item.Product.Categories?
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

        public async Task<CartGetDTO> CreateAsync(CartItemCreateDTO dto)
        {
            var product = await productRepository.GetByIdAsync(dto.ProductId);

            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0.");

            var cartItems = await cartItemRepository.GetAllAsync();

            var existing = cartItems.FirstOrDefault(x => x.ProductId == dto.ProductId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                await cartItemRepository.UpdateAsync(existing);
            }
            else
            {
                await cartItemRepository.AddAsync(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            return await GetAllAsync();
        }

        public async Task<CartGetDTO> UpdateAsync(int id, CartItemUpdateDTO dto)
        {
            var item = await cartItemRepository.GetByIdAsync(id);

            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0.");

            item.Quantity = dto.Quantity;

            await cartItemRepository.UpdateAsync(item);

            return await GetAllAsync();
        }

        public async Task<CartGetDTO> DeleteAsync(int id)
        {
            var item = await cartItemRepository.GetByIdAsync(id);

            await cartItemRepository.DeleteAsync(id);

            return await GetAllAsync();
        }

        public async Task DeleteAllAsync()
        {
            var items = await cartItemRepository.GetAllAsync();

            foreach (var item in items)
            {
                await cartItemRepository.DeleteAsync(item.Id);
            }
        }

        private async Task<CartGetDTO> BuildCartDTO(List<CartItem> cartItems)
        {
            foreach (var ci in cartItems)
            {
                ci.Product = await productRepository.GetByIdAsyncWithCategories(ci.ProductId);
            }

            var promotions = await promotionRepository.GetAllWithIncludesAsync();

            return CartMapper.ToGetDTO(cartItems, promotions);
        }
    }
}