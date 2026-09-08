namespace AISocInvestigator.Application.Features.Workflow;


public sealed record InvestigationApprovalResult(bool Approved, string SuggestedAction, string? ActionReason, string InvestigationMessage, string WorkflowSessionId);

