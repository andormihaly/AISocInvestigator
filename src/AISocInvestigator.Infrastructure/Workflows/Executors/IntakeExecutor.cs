using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Infrastructure.AgentFramework;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows.Executors;

public sealed partial class IntakeExecutor(IAgentFactory agentFactory, IWorkflowSessionManager workflowSessionManager) : Executor("IntakeExecutor")
{

    [MessageHandler]
    private async ValueTask<IntakeResult> HandleAsync( WorkflowRequest request, IWorkflowContext context, CancellationToken cancellationToken)
    {
        var agent = await agentFactory.GetIntakeAgentAsync(cancellationToken);

        var sessions = await workflowSessionManager.GetAsync(request.WorkflowSessionId, cancellationToken)
            ?? throw new InvalidOperationException($"Workflow session '{request.WorkflowSessionId}' was not found.");

        var response = await agent.RunAsync<IntakeResult>(
            request.Message,
            sessions.IntakeSession,
            cancellationToken: cancellationToken);

        return response.Result with { WorkflowSessionId = request.WorkflowSessionId };
    }

    protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
    {
        return protocolBuilder.ConfigureRoutes(routes => routes.AddHandler<WorkflowRequest, IntakeResult>(HandleAsync));
    }
}