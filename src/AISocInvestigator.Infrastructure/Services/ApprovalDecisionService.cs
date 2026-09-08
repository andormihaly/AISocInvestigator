using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Interfaces;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AISocInvestigator.Infrastructure.AgentFramework;

public sealed class ApprovalDecisionService(IAgentFactory agentFactory) : IApprovalDecisionService
{
    public async Task<ApprovalDecision> DecideAsync(string userResponse, InvestigationResult pendingInvestigation, CancellationToken cancellationToken = default)
    {
        var agent = await agentFactory.GetApprovalAgentAsync(cancellationToken);

        var message = $"""
                  Suggested action: {pendingInvestigation.SuggestedAction}
                  Reason: {pendingInvestigation.ActionReason}
                  Human response: {userResponse}
                  """;

        var serializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Web);
        serializerOptions.Converters.Add(new JsonStringEnumConverter());

        var options = new AgentRunOptions
        {
            ResponseFormat = ChatResponseFormat.ForJsonSchema<ApprovalDecision>()
        };

        var response = await agent.RunAsync<ApprovalDecision>(message, serializerOptions: serializerOptions, options: options, cancellationToken: cancellationToken);

        return response.Result;
    }
}