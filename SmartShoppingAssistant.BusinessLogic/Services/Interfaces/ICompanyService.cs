using SmartShoppingAssistant.BusinessLogic.DTOs.Company;
using SmartShoppingAssistant.BusinessLogic.Models;
using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BusinessLogic.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<List<CompanyGetDTO>> GetAllAsync(bool approvedOnly);
        Task<CompanyGetDTO> GetByIdAsync(int id);
        Task<CompanyGetDTO> GetBySlugAsync(string slug);
        Task<CompanyGetDTO> CreateAsync(CompanyCreateDTO dto, CompanyStatus status);
        Task<CompanyGetDTO> UpdateAsync(int id, CompanyUpdateDTO dto, CurrentUser user);
        Task<CompanyGetDTO> UpdateStatusAsync(int id, CompanyStatusUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
