using AISocInvestigator.Application.Features.Workflow;

namespace AISocInvestigator.Application.Interfaces;

public interface IApprovalDecisionService
{
    Task<ApprovalDecision> DecideAsync(string userResponse, InvestigationResult pendingInvestigation, CancellationToken cancellationToken = default);
}