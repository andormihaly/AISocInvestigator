using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Features.Chat;
using AISocInvestigator.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace AISocInvestigator.Api.Controllers;

public sealed class ChatController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string message, CancellationToken cancellationToken)
    {
        var result = await Bus.InvokeAsync<Result<ChatResponse>>(new AskChat(message), cancellationToken);

        return HandleResult(result);
    }
}