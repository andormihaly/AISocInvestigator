using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using AISocInvestigator.Infrastructure.AgentFramework;
using AISocInvestigator.Infrastructure.Telemetry;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Microsoft.Agents.AI.Workflows;
using System.Diagnostics;

namespace AISocInvestigator.Infrastructure.Workflows.Executors;

public sealed partial class InvestigatorExecutor(IAgentFactory agentFactory, IWorkflowSessionManager workflowSessionManager) : Executor("InvestigatorExecutor")
{
    [MessageHandler]
    private async ValueTask<WorkflowResponse> HandleAsync(IntakeResult input, IWorkflowContext context, CancellationToken cancellationToken)
    {
        using var activity = Telemetry.Telemetry.ActivitySource.StartActivity("Workflow.Investigation");

        activity?.SetTag("workflow.session_id", input.WorkflowSessionId);
        activity?.SetTag("agent.name", "soc-investigator-agent");
        activity?.SetTag("workflow.intent", input.Intent.ToString());

        try
        {
            var agent = await agentFactory.GetInvestigatorAgentAsync(cancellationToken);

            var sessions = await workflowSessionManager.GetAsync(input.WorkflowSessionId, cancellationToken)
                ?? throw new InvalidOperationException($"Workflow session '{input.WorkflowSessionId}' was not found.");

            var message = $"""
                          Intent: {input.Intent}
                          Incident ID: {input.IncidentId}
                          User goal: {input.UserGoal}
                          """;

            var response = await agent.RunAsync(message, sessions.InvestigatorSession, cancellationToken: cancellationToken);

            activity?.SetStatus(ActivityStatusCode.Ok);

            return new WorkflowResponse(response.Text);
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
        return protocolBuilder.ConfigureRoutes(routes => routes.AddHandler<IntakeResult, WorkflowResponse>(HandleAsync));
    }
}