using AISocInvestigator.Application.Features.Workflow;
using AISocInvestigator.Application.Features.Workflow.Handlers;
using AISocInvestigator.Application.Interfaces;
using AISocInvestigator.Infrastructure.Telemetry;
using AISocInvestigator.Infrastructure.Workflows.Sessions;
using Microsoft.Agents.AI.Workflows;

namespace AISocInvestigator.Infrastructure.Workflows;

public sealed class SocWorkflow(SocWorkflowDefinition definition, IWorkflowSessionManager workflowSessionManager, IWorkflowSessionFactory workflowSessionFactory, IApprovalDecisionService approvalDecisionService) : ISocWorkflow
{
    public async Task<WorkflowResponse> ExecuteAsync(WorkflowRequest request, CancellationToken cancellationToken = default)
    {
        using var activity = Telemetry.Telemetry.ActivitySource.StartActivity("Workflow.Execute");

        activity?.SetTag("workflow.session_id", request.WorkflowSessionId);
        activity?.SetTag("workflow.name", definition.Name);

        var sessions = await workflowSessionManager.GetAsync(request.WorkflowSessionId, cancellationToken);

        if (sessions is null)
        {
            sessions = await workflowSessionFactory.CreateAsync(cancellationToken);
            await workflowSessionManager.SetAsync(request.WorkflowSessionId, sessions, cancellationToken);
            activity?.SetTag("workflow.session.created", true);
        }
        else
        {
            activity?.SetTag("workflow.session.created", false);
        }

        try
        {
            WorkflowResponse response;

            if (sessions.Run is null)
            {
                response = await StartAsync(request, sessions, cancellationToken);
            }
            else
            {
                response = await HandlePendingRunAsync(request, sessions, cancellationToken);
            }

            activity?.SetStatus(System.Diagnostics.ActivityStatusCode.Ok);

            return response;
        }
        catch (Exception exception)
        {
            activity?.SetStatus(System.Diagnostics.ActivityStatusCode.Error, exception.Message);
            activity?.AddException(exception);
            throw;
        }
    }

    private async Task<WorkflowResponse> HandlePendingRunAsync(WorkflowRequest request, WorkflowSessions sessions, CancellationToken cancellationToken)
    {
        var run = sessions.Run ?? throw new InvalidOperationException("There is no pending workflow run.");

        var requestInfo = run.OutgoingEvents.OfType<RequestInfoEvent>().LastOrDefault() ?? throw new InvalidOperationException("Pending workflow does not contain a human approval request.");

        if (!requestInfo.Request.TryGetDataAs<InvestigationResult>(out var investigation))
        {
            throw new InvalidOperationException("Human approval request does not contain an InvestigationResult.");
        }

        var decision = await approvalDecisionService.DecideAsync(request.Message, investigation, cancellationToken);

        return decision.Decision switch
        {
            ApprovalDecisionType.Approved => await ResumeAsync(request, sessions, requestInfo, investigation, true, cancellationToken),
            ApprovalDecisionType.Rejected => await ResumeAsync(request, sessions, requestInfo, investigation, false, cancellationToken),
            ApprovalDecisionType.Other => await ExecuteNormalMessageAsync(request, sessions, cancellationToken),
            _ => throw new InvalidOperationException($"Unsupported approval decision '{decision.Decision}'.")
        };
    }
    private async Task<WorkflowResponse> ResumeAsync(WorkflowRequest request, WorkflowSessions sessions, RequestInfoEvent requestInfo, InvestigationResult investigation, bool approved, CancellationToken cancellationToken)
    {
        var run = sessions.Run ?? throw new InvalidOperationException("There is no pending workflow run to resume.");

        var approvalResult = new InvestigationApprovalResult(approved, investigation.SuggestedAction!, investigation.ActionReason, investigation.Message, request.WorkflowSessionId);

        var externalResponse = requestInfo.Request.CreateResponse(approvalResult);

        await run.ResumeAsync([externalResponse], cancellationToken);

        var output = run.NewEvents.OfType<WorkflowOutputEvent>().Select(x => x.As<WorkflowResponse>()).LastOrDefault(x => x is not null);

        if (output is null)
        {
            throw new InvalidOperationException("Resumed workflow did not produce a WorkflowResponse.");
        }

        await run.DisposeAsync();

        sessions = sessions with { Run = null };
        await workflowSessionManager.SetAsync(request.WorkflowSessionId, sessions, cancellationToken);

        return output;
    }
    private async Task<WorkflowResponse> StartAsync(WorkflowRequest request, WorkflowSessions sessions, CancellationToken cancellationToken)
    {
        var run = await InProcessExecution.RunAsync(definition.CreateWorkflow(), request, cancellationToken: cancellationToken);

        var output = run.OutgoingEvents.OfType<WorkflowOutputEvent>().Select(x => x.As<WorkflowResponse>()).LastOrDefault(x => x is not null);

        if (output is not null)
        {
            await run.DisposeAsync();
            return output;
        }

        var requestInfo = run.OutgoingEvents.OfType<RequestInfoEvent>().LastOrDefault();

        if (requestInfo is null)
        {
            await run.DisposeAsync();
            throw new InvalidOperationException("Workflow did not produce an output or human approval request.");
        }

        if (!requestInfo.Request.TryGetDataAs<InvestigationResult>(out var investigation))
        {
            await run.DisposeAsync();
            throw new InvalidOperationException("Human approval request does not contain an InvestigationResult.");
        }

        sessions = sessions with { Run = run };
        await workflowSessionManager.SetAsync(request.WorkflowSessionId, sessions, cancellationToken);

        return CreateApprovalResponse(investigation);
    }

  

    private async Task<WorkflowResponse> ExecuteNormalMessageAsync(WorkflowRequest request, WorkflowSessions sessions, CancellationToken cancellationToken)
    {
        await using var run = await InProcessExecution.RunAsync(definition.CreateWorkflow(), request, cancellationToken: cancellationToken);

        var output = run.OutgoingEvents.OfType<WorkflowOutputEvent>().Select(x => x.As<WorkflowResponse>()).LastOrDefault(x => x is not null);

        if (output is not null)
        {
            return output;
        }

        var requestInfo = run.OutgoingEvents.OfType<RequestInfoEvent>().LastOrDefault();

        if (requestInfo is not null)
        {
            return new WorkflowResponse("There is already an action waiting for your approval. Please approve or reject the existing action before starting another action requiring approval.");
        }

        throw new InvalidOperationException("Workflow did not produce an output.");
    }

    private static WorkflowResponse CreateApprovalResponse(InvestigationResult investigation)
    {
        return new WorkflowResponse($"{investigation.Message}\n\nSuggested action: {investigation.SuggestedAction}\nReason: {investigation.ActionReason}\n\nDo you approve?");
    }
}