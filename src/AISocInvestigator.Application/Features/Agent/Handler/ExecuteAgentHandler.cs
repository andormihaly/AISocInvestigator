using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Application.Models;

namespace AISocInvestigator.Application.Features.Agent;

public sealed class ExecuteAgentHandler(IAIAgentService agentService)
{
    public async Task<Result<ChatResponse>> Handle(ExecuteAgent request, CancellationToken cancellationToken)
    {
        return await agentService.ExecuteAsync(request.Request.Message, cancellationToken);
    }
}