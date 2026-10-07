using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public string? CardLast4 { get; set; }          // Only the last digits are kept, never the card

        public string ShippingFullName { get; set; } = null!;
        public string ShippingPhone { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingCounty { get; set; } = null!;
        public string ShippingPostalCode { get; set; } = null!;
        public string? Notes { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }           // Positive amount taken off the subtotal
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }

        public List<OrderPromotion> AppliedPromotions { get; set; } = [];
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    // Snapshot of a promotion as it was applied when the order was placed
    public class OrderPromotion
    {
        public int PromotionId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Discount { get; set; }
    }
}
