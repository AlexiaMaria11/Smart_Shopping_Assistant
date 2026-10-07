using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // The product can be edited or deleted later, so everything shown on the receipt is copied here
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        public OrderStatus Status { get; set; }
    }
}
