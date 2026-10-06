namespace SmartShoppingAssistant.DataAccess.Entities.Enums
{
    public enum CompanyStatus
    {
        Pending = 0,                // Registered, waiting for admin approval
        Approved = 1,               // Visible in the store, can sell
        Rejected = 2,
        Suspended = 3               // Was approved, temporarily hidden by admin
    }
}
