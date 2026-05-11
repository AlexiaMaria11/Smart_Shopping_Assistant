using Microsoft.Agents.AI;

namespace SmartShoppingAssistant.BusinessLogic.Agents;

public interface ISuggestionComposerAgent
{
    ChatClientAgent Build(
        string cartJson,
        string categoriesJson,
        string promotionAnalysisJson);
}