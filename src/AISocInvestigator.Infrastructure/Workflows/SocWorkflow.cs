using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Infrastructure.Telemetry;
using AISocInvestigator.Infrastructure.Workflows.Sessions;

namespace AISocInvestigator.Infrastructure.Workflows;

public sealed class SocWorkflow(SocWorkflowDefinition definition, IWorkflowRunner workflowRunner, IWorkflowSessionManager workflowSessionManager, IWorkflowSessionFactory workflowSessionFactory) : ISocWorkflow
{
    public async Task<WorkflowResponse> ExecuteAsync(WorkflowRequest request, CancellationToken cancellationToken = default)
    {
        using var activity = Telemetry.Telemetry.ActivitySource.StartActivity("Workflow.Execute");

        activity?.SetTag("workflow.session_id", request.WorkflowSessionId);
        activity?.SetTag("workflow.name", definition.Name);

        var sessions = await workflowSessionManager.GetAsync(request.WorkflowSessionId, cancellationToken);

        if (sessions is null)
        {
            sessions = await workflowSessionFactory.CreateAsync(cancellationToken);
            await workflowSessionManager.SetAsync(request.WorkflowSessionId, sessions, cancellationToken);
            activity?.SetTag("workflow.session.created", true);
        }
        else
        {
            activity?.SetTag("workflow.session.created", false);
        }

        try
        {
            var response = await workflowRunner.ExecuteAsync<WorkflowRequest, WorkflowResponse>(definition, request, cancellationToken);

            activity?.SetStatus(System.Diagnostics.ActivityStatusCode.Ok);

            return response;
        }
        catch (Exception exception)
        {
            activity?.SetStatus(System.Diagnostics.ActivityStatusCode.Error, exception.Message);
            activity?.AddException(exception);

            throw;
        }
    }
}