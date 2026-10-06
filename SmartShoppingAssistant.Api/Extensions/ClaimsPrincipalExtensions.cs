using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services;
using SmartShoppingAssistant.DataAccess.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartShoppingAssistant.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal principal)
    {
        var sub = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return int.TryParse(sub, out var id)
            ? id
            : throw new UnauthorizedAccessException("You need to sign in.");
    }

    public static CurrentUser ToCurrentUser(this ClaimsPrincipal principal)
    {
        var role = principal.FindFirstValue("role") ?? Roles.Customer;
        var companyClaim = principal.FindFirstValue(AuthService.CompanyIdClaim);
        int? companyId = int.TryParse(companyClaim, out var parsed) ? parsed : null;

        return new CurrentUser(principal.GetUserId(), role, companyId);
    }
}
