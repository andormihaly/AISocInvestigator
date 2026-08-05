using AISocInvestigator.Infrastructure.AgentFramework;

namespace AISocInvestigator.Infrastructure.Workflows.Sessions;

public sealed class WorkflowSessionFactory(IAgentFactory agentFactory) : IWorkflowSessionFactory
{
    public async Task<WorkflowSessions> CreateAsync(CancellationToken cancellationToken = default)
    {
        var intakeAgent = await agentFactory.GetIntakeAgentAsync(cancellationToken);
        var investigatorAgent = await agentFactory.GetInvestigatorAgentAsync(cancellationToken);
        var knowledgeAgent = await agentFactory.GetKnowledgeAgentAsync(cancellationToken);

        var intakeSession = await intakeAgent.CreateSessionAsync(cancellationToken);
        var investigatorSession = await investigatorAgent.CreateSessionAsync(cancellationToken);
        var knowledgeSession = await knowledgeAgent.CreateSessionAsync(cancellationToken);

        return new WorkflowSessions(intakeSession, investigatorSession, knowledgeSession);
    }
}