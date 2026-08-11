using AISocInvestigator.Application.Configuration;
using AISocInvestigator.Application.Interfaces;
using Azure.AI.Projects;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;

namespace AISocInvestigator.Infrastructure.AgentFramework;

public sealed class FoundryAgentFactory(AIProjectClient projectClient, IOptions<FoundryOptions> options) : IAgentFactory
{

    public Task<AIAgent> GetIntakeAgentAsync(CancellationToken cancellationToken = default) => GetAgentAsync(options.Value.IntakeAgentName, cancellationToken);

    public Task<AIAgent> GetInvestigatorAgentAsync(CancellationToken cancellationToken = default) => GetAgentAsync(options.Value.InvestigatorAgentName, cancellationToken);

    public Task<AIAgent> GetInvestigatorMCPAgentAsync(CancellationToken cancellationToken = default) => GetAgentAsync(options.Value.InvestigatorMCPAgentName, cancellationToken);

    public Task<AIAgent> GetKnowledgeAgentAsync(CancellationToken cancellationToken = default) => GetAgentAsync(options.Value.KnowledgeAgentName, cancellationToken);

    private async Task<AIAgent> GetAgentAsync(string agentName, CancellationToken cancellationToken)
    {
        var agentRecord = await projectClient.AgentAdministrationClient.GetAgentAsync(agentName, cancellationToken);
        return projectClient.AsAIAgent(agentRecord.Value);
    }
}