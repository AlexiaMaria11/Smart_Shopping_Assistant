namespace SmartShoppingAssistant.BusinessLogic.Helpers;

// A rule the user broke (duplicate name, empty cart...). Its message is safe to show in the UI.
public class BusinessException(string message, Exception? innerException = null)
    : Exception(message, innerException);
