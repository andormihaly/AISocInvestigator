using Microsoft.Extensions.Caching.Memory;

namespace AISocInvestigator.Infrastructure.Workflows.Sessions;

public sealed class WorkflowSessionManager(IMemoryCache cache) : IWorkflowSessionManager
{
    public Task<WorkflowSessions?> GetAsync(string workflowSessionId, CancellationToken cancellationToken = default)
    {
        cache.TryGetValue(workflowSessionId, out WorkflowSessions? sessions);

        return Task.FromResult(sessions);
    }

    public Task SetAsync(string workflowSessionId, WorkflowSessions sessions, CancellationToken cancellationToken = default)
    {
        cache.Set(
            workflowSessionId,
            sessions,
            new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });

        return Task.CompletedTask;
    }

    public void Remove(string workflowSessionId)
    {
        cache.Remove(workflowSessionId);
    }
}