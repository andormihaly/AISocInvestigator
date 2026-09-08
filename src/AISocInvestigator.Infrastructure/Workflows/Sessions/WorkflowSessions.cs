
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
namespace AISocInvestigator.Infrastructure.Workflows.Sessions;

public sealed record WorkflowSessions(AgentSession IntakeSession, AgentSession InvestigatorSession, AgentSession KnowledgeSession, Run Run=null);