using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.Order;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<OrderGetDTO>> PlaceOrder(CheckoutDTO dto)
        {
            var order = await orderService.PlaceOrderAsync(User.GetUserId(), dto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpGet("mine")]
        public async Task<ActionResult<List<OrderGetDTO>>> GetMine()
        {
            return Ok(await orderService.GetMineAsync(User.GetUserId()));
        }

        [Authorize(Roles = Roles.Seller)]
        [HttpGet("seller")]
        public async Task<ActionResult<List<OrderGetDTO>>> GetForSeller()
        {
            return Ok(await orderService.GetForSellerAsync(User.ToCurrentUser()));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public async Task<ActionResult<List<OrderGetDTO>>> GetAll()
        {
            return Ok(await orderService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderGetDTO>> GetById(int id)
        {
            return Ok(await orderService.GetAsync(id, User.ToCurrentUser()));
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<ActionResult<OrderGetDTO>> Cancel(int id)
        {
            return Ok(await orderService.CancelAsync(id, User.GetUserId()));
        }

        // Sellers move their own package; admins pass the company whose package they move
        [Authorize(Roles = $"{Roles.Admin},{Roles.Seller}")]
        [HttpPut("{id:int}/status")]
        public async Task<ActionResult<OrderGetDTO>> UpdateStatus(int id, OrderStatusUpdateDTO dto, int? companyId)
        {
            var user = User.ToCurrentUser();
            var targetCompany = user.IsSeller ? user.RequireCompanyId() : companyId
                ?? throw new ArgumentException("Choose the company whose package you want to update.");

            return Ok(await orderService.UpdateSellerStatusAsync(id, targetCompany, dto.Status, user));
        }
    }
}
