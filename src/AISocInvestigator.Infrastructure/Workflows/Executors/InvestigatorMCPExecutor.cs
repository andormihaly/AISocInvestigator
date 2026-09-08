using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Infrastructure.AgentFramework;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Microsoft.Agents.AI.Workflows;
using System.Diagnostics;

namespace AISocInvestigator.Infrastructure.Workflows.Executors;

public sealed partial class InvestigatorMCPExecutor(IAgentFactory agentFactory, IWorkflowSessionManager workflowSessionManager) : Executor("InvestigatorMCPExecutor")
{
    [MessageHandler]
    private async ValueTask<InvestigationResult> HandleAsync(IntakeResult input, IWorkflowContext context, CancellationToken cancellationToken)
    {
        using var activity = Telemetry.Telemetry.ActivitySource.StartActivity("Workflow.Investigation");

        activity?.SetTag("workflow.session_id", input.WorkflowSessionId);
        activity?.SetTag("agent.name", "soc-investigator-agent");
        activity?.SetTag("workflow.intent", input.Intent.ToString());

        try
        {
            var agent = await agentFactory.GetInvestigatorMCPAgentAsync(cancellationToken);

            var sessions = await workflowSessionManager.GetAsync(input.WorkflowSessionId, cancellationToken)
                ?? throw new InvalidOperationException($"Workflow session '{input.WorkflowSessionId}' was not found.");

            var message = $"""
                          Intent: {input.Intent}
                          Incident ID: {input.IncidentId}
                          User goal: {input.UserGoal}
                          """;

            var response = await agent.RunAsync<InvestigationResult>(message, sessions.InvestigatorSession, cancellationToken: cancellationToken);

            activity?.SetTag("workflow.suggested_action", response.Result.SuggestedAction);
            activity?.SetStatus(ActivityStatusCode.Ok);

            return response.Result;
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
        return protocolBuilder.ConfigureRoutes(routes => routes.AddHandler<IntakeResult, InvestigationResult>(HandleAsync));
    }
}