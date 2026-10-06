using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.Api.Extensions;
using SmartShoppingAssistant.BusinessLogic.DTOs.Auth;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO dto)
        {
            return Ok(await authService.RegisterAsync(dto));
        }

        [HttpPost("register-seller")]
        public async Task<ActionResult<AuthResponseDTO>> RegisterSeller(SellerRegisterDTO dto)
        {
            return Ok(await authService.RegisterSellerAsync(dto));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO dto)
        {
            return Ok(await authService.LoginAsync(dto));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO>> Me()
        {
            return Ok(await authService.GetUserAsync(User.GetUserId()));
        }
    }
}
