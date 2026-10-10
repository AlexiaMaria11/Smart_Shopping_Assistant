using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BusinessLogic.Helpers;

namespace SmartShoppingAssistant.BusinessLogic.Agents;

// Used when no AI key is configured, so the rest of the shop still starts and works
public sealed class DisabledChatClient : IChatClient
{
    private const string Message = "The AI assistant is not configured on this server.";

    public Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default) =>
        throw new BusinessException(Message);

    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default) =>
        throw new BusinessException(Message);

    public object? GetService(Type serviceType, object? serviceKey = null) => null;

    public void Dispose()
    {
    }
}
