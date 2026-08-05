namespace AISocInvestigator.Infrastructure.Workflows.Sessions;

public interface IWorkflowSessionManager
{
    Task<WorkflowSessions?> GetAsync(string workflowSessionId, CancellationToken cancellationToken = default);

    Task SetAsync(string workflowSessionId, WorkflowSessions sessions, CancellationToken cancellationToken = default);

    void Remove(string workflowSessionId);
}