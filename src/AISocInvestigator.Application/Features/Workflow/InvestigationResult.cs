namespace AISocInvestigator.Application.Features.Workflow;

public sealed record InvestigationResult(string Message, string? SuggestedAction = null, string? ActionReason = null);