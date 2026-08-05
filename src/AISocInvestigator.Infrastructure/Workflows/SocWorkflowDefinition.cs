using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Infrastructure.Workflows.Executors;
using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows;

public sealed class SocWorkflowDefinition(IntakeExecutor intakeExecutor, InvestigatorExecutor investigatorExecutor, KnowledgeExecutor knowledgeExecutor) : WorkflowDefinition
{
    public override string Name => nameof(SocWorkflowDefinition);

    public override Workflow CreateWorkflow()
    {
        var builder = new WorkflowBuilder(intakeExecutor);

        builder.AddEdge<IntakeResult>(intakeExecutor, investigatorExecutor, x => x!.Intent == WorkflowIntent.Investigation);

        builder.AddEdge<IntakeResult>(intakeExecutor, knowledgeExecutor, x => x!.Intent == WorkflowIntent.Knowledge);

        builder.WithOutputFrom(investigatorExecutor, knowledgeExecutor);

        return builder.Build();
    }

}