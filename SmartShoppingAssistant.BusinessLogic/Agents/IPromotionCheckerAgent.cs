using Microsoft.Agents.AI;

namespace SmartShoppingAssistant.BusinessLogic.Agents;

public interface IPromotionCheckerAgent
{
    ChatClientAgent Build(string cartJson);
}