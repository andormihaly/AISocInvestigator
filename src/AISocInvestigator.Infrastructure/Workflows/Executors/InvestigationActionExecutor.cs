using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using AISocInvestigator.Infrastructure.AgentFramework;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Microsoft.Agents.AI.Workflows;
using System.Diagnostics;

namespace AISocInvestigator.Infrastructure.Workflows.Executors;

public sealed partial class InvestigationActionExecutor(IAgentFactory agentFactory, IWorkflowSessionManager workflowSessionManager) : Executor("InvestigationActionExecutor")
{
    [MessageHandler]
    private async ValueTask<WorkflowResponse> HandleAsync(InvestigationApprovalResult input, IWorkflowContext context, CancellationToken cancellationToken)
    {
        if (!input.Approved)
        {
            return new WorkflowResponse("The suggested action was not approved.");
        }

        using var activity = Telemetry.Telemetry.ActivitySource.StartActivity("Workflow.InvestigationAction");

        activity?.SetTag("workflow.session_id", input.WorkflowSessionId);
        activity?.SetTag("agent.name", "soc-investigator-agent");
        activity?.SetTag("workflow.suggested_action", input.SuggestedAction);

        try
        {
            var agent = await agentFactory.GetInvestigatorMCPAgentAsync(cancellationToken);

            var sessions = await workflowSessionManager.GetAsync(input.WorkflowSessionId, cancellationToken)
                ?? throw new InvalidOperationException($"Workflow session '{input.WorkflowSessionId}' was not found.");

            var message = $"""
                      The user approved the following action.

                      Suggested action: {input.SuggestedAction}
                      Reason: {input.ActionReason}

                      Investigation:
                      {input.InvestigationMessage}

                      Execute the approved action using the available MCP tools.

                      If the approved action is CreateAlert:
                      - create a Microsoft Defender alert using the create_alert tool;
                      - generate a concise alert title based on the investigation;
                      - use the investigation findings as the alert description;
                      - choose an appropriate severity;
                      - choose an appropriate Microsoft Defender category;
                      - use a relevant impacted entity from the investigation;
                      - for an IP address use entityType "Ip" and entityIdentifier "address";
                      - do not create a Microsoft Sentinel incident.
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
        return protocolBuilder.ConfigureRoutes(routes => routes.AddHandler<InvestigationApprovalResult, WorkflowResponse>(HandleAsync));
    }
}