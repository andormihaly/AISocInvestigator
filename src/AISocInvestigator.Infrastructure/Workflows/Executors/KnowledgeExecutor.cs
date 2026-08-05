using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using AISocInvestigator.Infrastructure.AgentFramework;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows.Executors;

public sealed partial class KnowledgeExecutor(IAgentFactory agentFactory, IWorkflowSessionManager workflowSessionManager) : Executor("KnowledgeExecutor")
{
    [MessageHandler]
    private async ValueTask<WorkflowResponse> HandleAsync(IntakeResult input, IWorkflowContext context, CancellationToken cancellationToken)
    {
        var agent = await agentFactory.GetKnowledgeAgentAsync(cancellationToken);

        var sessions = await workflowSessionManager.GetAsync(input.WorkflowSessionId, cancellationToken)
            ?? throw new InvalidOperationException($"Workflow session '{input.WorkflowSessionId}' was not found.");

        var message = $""" Topic: {input.Topic} User goal: {input.UserGoal} """;

        var response = await agent.RunAsync(message, sessions.KnowledgeSession, cancellationToken: cancellationToken);

        return new WorkflowResponse(response.Text);
    }

    protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
    {
        return protocolBuilder.ConfigureRoutes(routes => routes.AddHandler<IntakeResult, WorkflowResponse>(HandleAsync));
    }
}