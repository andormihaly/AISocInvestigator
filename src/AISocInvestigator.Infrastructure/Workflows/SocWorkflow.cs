using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Infrastructure.Workflows.Sessions;

namespace AISocInvestigator.Infrastructure.Workflows;

public sealed class SocWorkflow(SocWorkflowDefinition definition, IWorkflowRunner workflowRunner, IWorkflowSessionManager workflowSessionManager,
    IWorkflowSessionFactory workflowSessionFactory) : ISocWorkflow
{
    public async Task<WorkflowResponse> ExecuteAsync(WorkflowRequest request, CancellationToken cancellationToken = default)
    {
        var sessions = await workflowSessionManager.GetAsync(request.WorkflowSessionId, cancellationToken);

        if (sessions is null)
        {
            sessions = await workflowSessionFactory.CreateAsync(cancellationToken);

            await workflowSessionManager.SetAsync(request.WorkflowSessionId, sessions, cancellationToken);
        }

        return await workflowRunner.ExecuteAsync<WorkflowRequest, WorkflowResponse>(
            definition,
            request,
            cancellationToken);
    }

}