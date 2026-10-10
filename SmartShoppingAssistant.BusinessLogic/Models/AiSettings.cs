namespace SmartShoppingAssistant.BusinessLogic.Models;

// Any OpenAI-compatible service works (OpenAI, Azure OpenAI, Google Gemini's OpenAI endpoint...)
public class AiSettings
{
    public const string SectionName = "OpenAI";

    public string? ApiKey { get; set; }
    public string Model { get; set; } = "gpt-4o-mini";
    // Leave empty for OpenAI; set it for any other OpenAI-compatible service
    public string? Endpoint { get; set; }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey);
}
