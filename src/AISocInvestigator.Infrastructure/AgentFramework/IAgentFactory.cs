using Azure.AI.Projects;
using Microsoft.Agents.AI;

namespace AISocInvestigator.Infrastructure.AgentFramework;

public interface IAgentFactory
{
    Task<AIAgent> GetIntakeAgentAsync(CancellationToken cancellationToken = default);

    Task<AIAgent> GetInvestigatorAgentAsync(CancellationToken cancellationToken = default);

    Task<AIAgent> GetKnowledgeAgentAsync(CancellationToken cancellationToken = default);
}