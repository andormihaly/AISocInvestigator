using AISocInvestigator.Application.Core;
using AISocInvestigator.Application.Models;

public interface IAIAgentService
{
    Task<Result<ChatResponse>> ExecuteAsync(string message, string? previousResponseId, CancellationToken cancellationToken = default);
}