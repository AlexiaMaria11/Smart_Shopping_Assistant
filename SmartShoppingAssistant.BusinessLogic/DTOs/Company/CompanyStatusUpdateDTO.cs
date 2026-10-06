using SmartShoppingAssistant.DataAccess.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Company
{
    public class CompanyStatusUpdateDTO
    {
        public CompanyStatus Status { get; set; }
        [Range(0, 50)]
        public decimal CommissionPercent { get; set; }
    }
}
