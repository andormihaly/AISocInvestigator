using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Models;

namespace AISocInvestigator.Application.Interfaces;

public interface IAIChatService
{
    Task<Result<ChatResponse>> AskAsync(string message, string? previousResponseId, CancellationToken cancellationToken = default);
}