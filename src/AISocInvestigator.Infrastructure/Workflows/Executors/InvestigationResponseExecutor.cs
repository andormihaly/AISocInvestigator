using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows.Executors;

public sealed partial class InvestigationResponseExecutor() : Executor("InvestigationResponseExecutor")
{
    [MessageHandler]
    private ValueTask<WorkflowResponse> HandleAsync(InvestigationResult input, IWorkflowContext context, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(new WorkflowResponse(input.Message));
    }

    protected override ProtocolBuilder ConfigureProtocol(ProtocolBuilder protocolBuilder)
    {
        return protocolBuilder.ConfigureRoutes(routes => routes.AddHandler<InvestigationResult, WorkflowResponse>(HandleAsync));
    }
}