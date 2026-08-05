using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using AISocInvestigator.Application.Interfaces;

namespace AISocInvestigator.Application.Features.Workflow;

public sealed class ExecuteWorkflowHandler(ISocWorkflow workflow)
{
    public async Task<Result<WorkflowResponse>> Handle(ExecuteWorkflow request, CancellationToken cancellationToken)
    {
        var response = await workflow.ExecuteAsync(request.Request, cancellationToken);

        return Result<WorkflowResponse>.Success(response);
    }
}