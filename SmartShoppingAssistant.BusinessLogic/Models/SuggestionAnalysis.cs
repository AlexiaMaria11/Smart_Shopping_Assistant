using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SmartShoppingAssistant.BusinessLogic.Models;

[Description("Product suggestions generated for the current cart")]
public sealed class SuggestionAnalysis
{
    [JsonPropertyName("suggestions")]
    public List<ProductSuggestion> Suggestions { get; set; } = [];
}

public sealed class ProductSuggestion
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = "";

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = "";

    [JsonPropertyName("promotionRelated")]
    public bool PromotionRelated { get; set; }

    [JsonPropertyName("estimatedSavings")]
    public decimal? EstimatedSavings { get; set; }
}