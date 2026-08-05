using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;

namespace AISocInvestigator.Application.Interfaces;

public interface ISocWorkflow
{
    Task<WorkflowResponse> ExecuteAsync(WorkflowRequest request, CancellationToken cancellationToken = default);
}