using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Models;

// The signed-in user as seen by the services, built from the JWT claims by the API layer
public sealed record CurrentUser(int Id, string Role, int? CompanyId)
{
    public bool IsAdmin => Role == Roles.Admin;
    public bool IsSeller => Role == Roles.Seller;

    // Sellers may only touch data of their own company; admins may touch everything
    public void EnsureCanManageCompany(int companyId)
    {
        if (IsAdmin)
            return;

        if (!IsSeller || CompanyId != companyId)
            throw new UnauthorizedAccessException("You can only manage your own company's data.");
    }

    public int RequireCompanyId() =>
        CompanyId ?? throw new UnauthorizedAccessException("Your account is not linked to a company.");
}
