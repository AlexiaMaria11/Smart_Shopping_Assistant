namespace SmartShoppingAssistant.DataAccess.Entities.Enums
{
    // Used for a whole order and for each seller's items inside it
    public enum OrderStatus
    {
        Pending = 0,            // Placed, the seller has not looked at it yet
        Confirmed = 1,          // The seller is preparing the package
        Shipped = 2,            // Handed to the courier
        Delivered = 3,
        Cancelled = 4
    }
}
