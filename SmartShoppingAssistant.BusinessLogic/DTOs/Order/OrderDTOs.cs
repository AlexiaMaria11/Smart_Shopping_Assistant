using SmartShoppingAssistant.DataAccess.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartShoppingAssistant.BusinessLogic.DTOs.Order
{
    public class CheckoutDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Please enter the full name of the person receiving the package.")]
        public string ShippingFullName { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(\+4|004)?07\d{8}$", ErrorMessage = "Please enter a Romanian mobile number, for example 0722123456.")]
        public string ShippingPhone { get; set; } = null!;

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Please enter the street and number.")]
        public string ShippingAddress { get; set; } = null!;

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string ShippingCity { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string ShippingCounty { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "The postal code has 6 digits.")]
        public string ShippingPostalCode { get; set; } = null!;

        [StringLength(500)]
        public string? Notes { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        // Required only when paying by card
        public CardPaymentDTO? Card { get; set; }
    }

    public class CardPaymentDTO
    {
        [Required]
        public string HolderName { get; set; } = null!;
        [Required]
        public string Number { get; set; } = null!;
        [Range(1, 12)]
        public int ExpiryMonth { get; set; }
        [Range(2000, 2100)]
        public int ExpiryYear { get; set; }
        [Required]
        public string Cvv { get; set; } = null!;
    }

    public class OrderItemGetDTO
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public OrderStatus Status { get; set; }
    }

    public class OrderPromotionDTO
    {
        public string Name { get; set; } = null!;
        public decimal Discount { get; set; }
    }

    public class OrderGetDTO
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public bool CanCancel { get; set; }

        public string CustomerName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;

        public PaymentMethod PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public string? CardLast4 { get; set; }

        public string ShippingFullName { get; set; } = null!;
        public string ShippingPhone { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public string ShippingCity { get; set; } = null!;
        public string ShippingCounty { get; set; } = null!;
        public string ShippingPostalCode { get; set; } = null!;
        public string? Notes { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        public List<OrderPromotionDTO> AppliedPromotions { get; set; } = [];

        public List<OrderItemGetDTO> Items { get; set; } = [];
    }

    public class OrderStatusUpdateDTO
    {
        public OrderStatus Status { get; set; }
    }
}
