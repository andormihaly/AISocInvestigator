using AISocInvestigator.Infrastructure.Workflows;
public interface IWorkflowRunner
{
    Task<TOutput> ExecuteAsync<TInput, TOutput>(WorkflowDefinition workflow, TInput input, CancellationToken cancellationToken = default);
}