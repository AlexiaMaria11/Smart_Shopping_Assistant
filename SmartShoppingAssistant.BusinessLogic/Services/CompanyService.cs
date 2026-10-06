using SmartShoppingAssistant.BusinessLogic.DTOs.Company;
using SmartShoppingAssistant.BusinessLogic.Helpers;
using SmartShoppingAssistant.BusinessLogic.Mappers;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.BusinessLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BusinessLogic.Services;

public class CompanyService(ICompanyRepository companyRepository) : ICompanyService
{
    public const decimal DefaultCommissionPercent = 10m;

    public async Task<List<CompanyGetDTO>> GetAllAsync(bool approvedOnly)
    {
        var companies = await companyRepository.GetAllWithProductCountAsync(approvedOnly);
        return companies.Select(CompanyMapper.ToGetDTO).ToList();
    }

    public async Task<CompanyGetDTO> GetByIdAsync(int id)
    {
        var company = await companyRepository.GetByIdAsync(id);
        return await ToDtoWithCountAsync(company);
    }

    public async Task<CompanyGetDTO> GetBySlugAsync(string slug)
    {
        var company = await companyRepository.GetBySlugAsync(slug);
        if (company.Status != CompanyStatus.Approved)
            throw new KeyNotFoundException($"Company '{slug}' not found");
        return await ToDtoWithCountAsync(company);
    }

    public async Task<CompanyGetDTO> CreateAsync(CompanyCreateDTO dto, CompanyStatus status)
    {
        if (await companyRepository.NameExistsAsync(dto.Name.Trim()))
            throw new BusinessException($"A company named '{dto.Name}' already exists.");

        var company = CompanyMapper.ToEntity(dto);
        company.Slug = await UniqueSlugAsync(company.Name);
        company.Status = status;
        company.CommissionPercent = DefaultCommissionPercent;

        var created = await companyRepository.AddAsync(company);
        return CompanyMapper.ToGetDTO(created);
    }

    public async Task<CompanyGetDTO> UpdateAsync(int id, CompanyUpdateDTO dto, CurrentUser user)
    {
        user.EnsureCanManageCompany(id);

        if (await companyRepository.NameExistsAsync(dto.Name.Trim(), id))
            throw new BusinessException($"A company named '{dto.Name}' already exists.");

        var company = await companyRepository.GetByIdAsync(id);
        var nameChanged = company.Name != dto.Name.Trim();
        CompanyMapper.UpdateEntity(company, dto);
        if (nameChanged)
            company.Slug = await UniqueSlugAsync(company.Name, id);

        await companyRepository.UpdateAsync(company);
        return await ToDtoWithCountAsync(company);
    }

    public async Task<CompanyGetDTO> UpdateStatusAsync(int id, CompanyStatusUpdateDTO dto)
    {
        var company = await companyRepository.GetByIdAsync(id);
        company.Status = dto.Status;
        company.CommissionPercent = dto.CommissionPercent;
        await companyRepository.UpdateAsync(company);
        return await ToDtoWithCountAsync(company);
    }

    public async Task DeleteAsync(int id)
    {
        if (await companyRepository.CountProductsAsync(id) > 0)
            throw new BusinessException("This company still has products. Delete or move them first.");

        await companyRepository.DeleteAsync(id);
    }

    private async Task<CompanyGetDTO> ToDtoWithCountAsync(Company company)
    {
        var dto = CompanyMapper.ToGetDTO(company);
        dto.ProductCount = await companyRepository.CountProductsAsync(company.Id);
        return dto;
    }

    private async Task<string> UniqueSlugAsync(string name, int? excludeId = null)
    {
        var baseSlug = SlugHelper.ToSlug(name);
        var slug = baseSlug;
        var suffix = 2;
        while (await companyRepository.SlugExistsAsync(slug, excludeId))
            slug = $"{baseSlug}-{suffix++}";
        return slug;
    }
}
