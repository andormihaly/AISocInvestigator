using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using AISocInvestigator.Infrastructure.AgentFramework;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows.Executors;

public sealed partial class InvestigatorExecutor(IAgentFactory agentFactory, IWorkflowSessionManager workflowSessionManager) : Executor("InvestigatorExecutor")
{
    [MessageHandler]
    private async ValueTask<WorkflowResponse> HandleAsync(IntakeResult input, IWorkflowContext context, CancellationToken cancellationToken)
    {
        var agent = await agentFactory.GetInvestigatorAgentAsync(cancellationToken);

        var sessions = await workflowSessionManager.GetAsync(input.WorkflowSessionId, cancellationToken)
            ?? throw new InvalidOperationException($"Workflow session '{input.WorkflowSessionId}' was not found.");

        var message = $""" Intent: {input.Intent} Incident ID: {input.IncidentId} User goal: {input.UserGoal}""";

        var response = await agent.RunAsync(message, sessions.InvestigatorSession, cancellationToken: cancellationToken);

        return new WorkflowResponse(response.Text);
    }

    protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
    {
        return protocolBuilder.ConfigureRoutes(routes => routes.AddHandler<IntakeResult, WorkflowResponse>(HandleAsync));
    }
}