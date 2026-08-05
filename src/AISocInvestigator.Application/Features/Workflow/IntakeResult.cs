namespace AISocInvestigator.Application.Features.Workflow;

public enum WorkflowIntent
{
    Investigation,
    Knowledge,
    Unsupported
}

public sealed record IntakeResult(string WorkflowSessionId, WorkflowIntent Intent, string? IncidentId, string? Topic, string? UserGoal);