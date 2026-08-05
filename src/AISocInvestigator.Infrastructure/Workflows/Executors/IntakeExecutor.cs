using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Infrastructure.AgentFramework;
using AISocInvestigator.Infrastructure.Telemetry;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Microsoft.Agents.AI.Workflows;
using System.Diagnostics;

namespace AISocInvestigator.Infrastructure.Workflows.Executors;

public sealed partial class IntakeExecutor(IAgentFactory agentFactory, IWorkflowSessionManager workflowSessionManager) : Executor("IntakeExecutor")
{
    [MessageHandler]
    private async ValueTask<IntakeResult> HandleAsync(WorkflowRequest request, IWorkflowContext context, CancellationToken cancellationToken)
    {
        using var activity = Telemetry.Telemetry.ActivitySource.StartActivity("Workflow.Intake");

        activity?.SetTag("workflow.session_id", request.WorkflowSessionId);
        activity?.SetTag("agent.name", "soc-intake-agent");

        try
        {
            var agent = await agentFactory.GetIntakeAgentAsync(cancellationToken);

            var sessions = await workflowSessionManager.GetAsync(request.WorkflowSessionId, cancellationToken)
                ?? throw new InvalidOperationException($"Workflow session '{request.WorkflowSessionId}' was not found.");

            var response = await agent.RunAsync<IntakeResult>(request.Message, sessions.IntakeSession, cancellationToken: cancellationToken);

            activity?.SetTag("workflow.intent", response.Result.Intent.ToString());
            activity?.SetStatus(ActivityStatusCode.Ok);

            return response.Result with { WorkflowSessionId = request.WorkflowSessionId };
        }
        catch (Exception exception)
        {
            activity?.SetStatus(ActivityStatusCode.Error, exception.Message);
            activity?.AddException(exception);

            throw;
        }
    }

    protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
    {
        return protocolBuilder.ConfigureRoutes(routes => routes.AddHandler<WorkflowRequest, IntakeResult>(HandleAsync));
    }
}