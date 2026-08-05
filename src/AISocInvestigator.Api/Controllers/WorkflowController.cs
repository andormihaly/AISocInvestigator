using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace AISocInvestigator.Api.Controllers;

public sealed class WorkflowController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<WorkflowResponse>> Post(WorkflowRequest request, CancellationToken cancellationToken)
    {
        var result = await Bus.InvokeAsync<Result<WorkflowResponse>>(new ExecuteWorkflow(request), cancellationToken);
        return HandleResult(result);
    }
}