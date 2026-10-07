using SmartShoppingAssistant.BusinessLogic.DTOs.Order;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

public interface IOrderService
{
    Task<OrderGetDTO> PlaceOrderAsync(int userId, CheckoutDTO dto);
    Task<List<OrderGetDTO>> GetMineAsync(int userId);
    Task<List<OrderGetDTO>> GetForSellerAsync(CurrentUser user);
    Task<List<OrderGetDTO>> GetAllAsync();
    Task<OrderGetDTO> GetAsync(int id, CurrentUser user);
    Task<OrderGetDTO> CancelAsync(int id, int userId);
    Task<OrderGetDTO> UpdateSellerStatusAsync(int id, int companyId, OrderStatus status, CurrentUser user);
}
