using Microsoft.AspNetCore.Identity;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess;

public static partial class DataSeeder
{
    // Demo accounts so every role can be tried right away (development data only)
    private const string AdminPassword = "Admin1234";
    private const string SellerPassword = "Seller1234";
    private const string CustomerPassword = "Client1234";

    private static async Task SeedUsersAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        Dictionary<string, Company> companies)
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        await EnsureUserAsync(userManager, "admin@smartshop.ro", "Platform Admin", AdminPassword, Roles.Admin, null);
        await EnsureUserAsync(userManager, "client@smartshop.ro", "Maria Popescu", CustomerPassword, Roles.Customer, null);

        foreach (var company in companies.Values)
        {
            var email = company.ContactEmail ?? $"{company.Slug}@smartshop.ro";
            await EnsureUserAsync(userManager, email, $"{company.Name} Team", SellerPassword, Roles.Seller, company.Id);
        }
    }

    private static async Task EnsureUserAsync(
        UserManager<AppUser> userManager,
        string email,
        string fullName,
        string password,
        string role,
        int? companyId)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = fullName,
            CompanyId = companyId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new InvalidOperationException($"Could not seed user {email}: {string.Join(" ", result.Errors.Select(e => e.Description))}");

        await userManager.AddToRoleAsync(user, role);
    }
}
