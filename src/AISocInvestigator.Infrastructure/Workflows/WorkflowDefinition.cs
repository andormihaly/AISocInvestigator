using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows;

public abstract class WorkflowDefinition
{
    public abstract string Name { get; }

    public abstract Workflow CreateWorkflow();
}