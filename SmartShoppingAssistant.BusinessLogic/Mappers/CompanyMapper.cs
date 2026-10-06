using SmartShoppingAssistant.BusinessLogic.DTOs.Company;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BusinessLogic.Mappers
{
    public static class CompanyMapper
    {
        public static CompanyGetDTO ToGetDTO(Company company)
        {
            return new CompanyGetDTO
            {
                Id = company.Id,
                Name = company.Name,
                Slug = company.Slug,
                Description = company.Description,
                LogoUrl = company.LogoUrl,
                BannerUrl = company.BannerUrl,
                ContactEmail = company.ContactEmail,
                Website = company.Website,
                Status = company.Status,
                CommissionPercent = company.CommissionPercent,
                CreatedAt = company.CreatedAt,
                ProductCount = company.Products.Count
            };
        }

        public static Company ToEntity(CompanyCreateDTO dto)
        {
            return new Company
            {
                Name = dto.Name.Trim(),
                Description = dto.Description,
                LogoUrl = dto.LogoUrl,
                BannerUrl = dto.BannerUrl,
                ContactEmail = dto.ContactEmail,
                Website = dto.Website,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateEntity(Company company, CompanyUpdateDTO dto)
        {
            company.Name = dto.Name.Trim();
            company.Description = dto.Description;
            company.LogoUrl = dto.LogoUrl;
            company.BannerUrl = dto.BannerUrl;
            company.ContactEmail = dto.ContactEmail;
            company.Website = dto.Website;
        }
    }
}
