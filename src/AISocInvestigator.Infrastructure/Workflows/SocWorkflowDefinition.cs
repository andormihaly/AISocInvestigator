using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Infrastructure.Workflows.Executors;
using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows;

public sealed class SocWorkflowDefinition(
    IntakeExecutor intakeExecutor,
    InvestigatorMCPExecutor investigatorMCPExecutor,
    KnowledgeExecutor knowledgeExecutor,
    InvestigationResponseExecutor investigationResponseExecutor,
    InvestigationActionExecutor investigationActionExecutor) : WorkflowDefinition
{
    public override string Name => nameof(SocWorkflowDefinition);

    public override Workflow CreateWorkflow()
    {
        var approvalPort = RequestPort.Create<InvestigationResult, InvestigationApprovalResult>("InvestigationApproval");

        var builder = new WorkflowBuilder(intakeExecutor);

        builder.AddEdge<IntakeResult>(intakeExecutor, investigatorMCPExecutor, x => x!.Intent == WorkflowIntent.Investigation);
        builder.AddEdge<IntakeResult>(intakeExecutor, knowledgeExecutor, x => x!.Intent == WorkflowIntent.Knowledge);
        builder.AddEdge<InvestigationResult>(investigatorMCPExecutor, investigationResponseExecutor, x => string.IsNullOrWhiteSpace(x!.SuggestedAction));
        builder.AddEdge<InvestigationResult>(investigatorMCPExecutor, approvalPort, x => !string.IsNullOrWhiteSpace(x!.SuggestedAction));
        builder.AddEdge(approvalPort, investigationActionExecutor);
        builder.WithOutputFrom(investigationResponseExecutor, investigationActionExecutor, knowledgeExecutor);

        return builder.Build();
    }
}