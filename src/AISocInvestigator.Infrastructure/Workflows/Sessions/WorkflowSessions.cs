using Microsoft.Agents.AI;

namespace AISocInvestigator.Infrastructure.Workflows.Sessions;

public sealed record WorkflowSessions(AgentSession IntakeSession, AgentSession InvestigatorSession, AgentSession KnowledgeSession);