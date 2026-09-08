namespace AISocInvestigator.Application.Features.Workflow;

public sealed record ApprovalInput(string UserResponse, string SuggestedAction, string? ActionReason);