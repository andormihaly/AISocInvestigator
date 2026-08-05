namespace AISocInvestigator.Infrastructure.Workflows.Sessions;

public interface IWorkflowSessionFactory
{
    Task<WorkflowSessions> CreateAsync(CancellationToken cancellationToken = default);
}