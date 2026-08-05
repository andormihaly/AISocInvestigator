using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows;

public sealed class WorkflowRunner : IWorkflowRunner
{
    public async Task<TOutput> ExecuteAsync<TInput, TOutput>(WorkflowDefinition workflow, TInput input, CancellationToken cancellationToken = default)
    {
        await using var run = await InProcessExecution.RunAsync(workflow.CreateWorkflow(), input, cancellationToken: cancellationToken);

        var output = run.OutgoingEvents.OfType<WorkflowOutputEvent>().Select(x => x.As<TOutput>()).LastOrDefault(x => x is not null);

        return output ?? throw new InvalidOperationException($"Workflow '{workflow.Name}' did not produce an output of type '{typeof(TOutput).Name}'.");
    }
}