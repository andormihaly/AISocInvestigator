using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Features.Agent;
using AISocInvestigator.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace AISocInvestigator.Api.Controllers;

public sealed class AgentController : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Post(ExecuteAgentRequest request, CancellationToken cancellationToken)
    {
        var result = await Bus.InvokeAsync<Result<ChatResponse>>(new ExecuteAgent(request), cancellationToken);
        return HandleResult(result);
    }
}