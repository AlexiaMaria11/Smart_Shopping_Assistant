using SmartShoppingAssistant.BusinessLogic.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartShoppingAssistant.BusinessLogic.DTOs.Auth;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    ICompanyService companyService,
    ICompanyRepository companyRepository,
    IOptions<JwtSettings> jwtOptions) : IAuthService
{
    public const string CompanyIdClaim = "companyId";

    public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
    {
        var user = await CreateUserAsync(dto, Roles.Customer, companyId: null);
        return await BuildResponseAsync(user);
    }

    // Creates the company (waiting for admin approval) together with the seller's account
    public async Task<AuthResponseDTO> RegisterSellerAsync(SellerRegisterDTO dto)
    {
        await EnsureEmailIsFreeAsync(dto.Email);

        var company = await companyService.CreateAsync(dto.Company, CompanyStatus.Pending);
        try
        {
            var user = await CreateUserAsync(dto, Roles.Seller, company.Id);
            return await BuildResponseAsync(user);
        }
        catch
        {
            await companyRepository.DeleteAsync(company.Id);
            throw;
        }
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedAccessException("Wrong email or password.");

        return await BuildResponseAsync(user);
    }

    public async Task<UserDTO> GetUserAsync(int userId)
    {
        var user = await userManager.Users
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new KeyNotFoundException("User not found");

        return await ToUserDtoAsync(user);
    }

    private async Task EnsureEmailIsFreeAsync(string email)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
            throw new BusinessException("An account with this email already exists.");
    }

    private async Task<AppUser> CreateUserAsync(RegisterDTO dto, string role, int? companyId)
    {
        await EnsureEmailIsFreeAsync(dto.Email);

        var user = new AppUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName.Trim(),
            CompanyId = companyId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new BusinessException(string.Join(" ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, role);
        return user;
    }

    private async Task<UserDTO> ToUserDtoAsync(AppUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var company = user.Company ?? (user.CompanyId.HasValue
            ? await companyRepository.GetByIdAsync(user.CompanyId.Value)
            : null);

        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            Role = roles.FirstOrDefault() ?? Roles.Customer,
            CompanyId = user.CompanyId,
            CompanyName = company?.Name,
            CompanyStatus = company?.Status
        };
    }

    private async Task<AuthResponseDTO> BuildResponseAsync(AppUser user)
    {
        var userDto = await ToUserDtoAsync(user);
        var settings = jwtOptions.Value;
        var expiresAt = DateTime.UtcNow.AddHours(settings.ExpiresHours);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, userDto.Email),
            new(JwtRegisteredClaimNames.Name, userDto.FullName),
            new("role", userDto.Role),
        };
        if (user.CompanyId.HasValue)
            claims.Add(new Claim(CompanyIdClaim, user.CompanyId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new AuthResponseDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt,
            User = userDto
        };
    }
}
