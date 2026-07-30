using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Application.Models;

namespace AISocInvestigator.Application.Features.Chat;

public sealed class AskChatHandler(IAIChatService chatService)
{
    public Task<Result<ChatResponse>> Handle(AskChat command, CancellationToken cancellationToken)
    {
        return chatService.AskAsync(command.Message, command.PreviousResponseId, cancellationToken);
    }
}