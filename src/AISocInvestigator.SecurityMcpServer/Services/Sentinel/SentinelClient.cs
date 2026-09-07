using AISocInvestigator.SecurityMcpServer.Configuration;
using AISocInvestigator.SecurityMcpServer.Models;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.SecurityInsights;
using Azure.ResourceManager.SecurityInsights.Models;
using Microsoft.Extensions.Options;

namespace AISocInvestigator.SecurityMcpServer.Services.Sentinel;

public sealed class SentinelClient(
    ArmClient armClient,
    IOptions<SentinelOptions> sentinelOptions,
    ILogger<SentinelClient> logger) : ISentinelClient
{
    private readonly SentinelOptions _options = sentinelOptions.Value;

    public async Task<SentinelIncident?> GetIncidentAsync(string id)
    {
        try
        {
            var resourceId =
                SecurityInsightsIncidentResource.CreateResourceIdentifier(
                    _options.SentinelSubscriptionId,
                    _options.ResourceGroupName,
                    _options.WorkspaceName,
                    id);

            var incidentResource =
                armClient.GetSecurityInsightsIncidentResource(resourceId);

            var response = await incidentResource.GetAsync();
            var data = response.Value.Data;

            return MapIncident(data);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to retrieve Sentinel incident {IncidentId}.",
                id);

            throw;
        }
    }

    public async Task<IReadOnlyList<SentinelIncident>> GetIncidentsAsync()
    {
        try
        {
            var workspaceResourceId = new ResourceIdentifier(
                $"/subscriptions/{_options.SentinelSubscriptionId}" +
                $"/resourceGroups/{_options.ResourceGroupName}" +
                "/providers/Microsoft.OperationalInsights" +
                $"/workspaces/{_options.WorkspaceName}");

            var incidents = new List<SentinelIncident>();

            await foreach (var incidentResource in armClient.GetSecurityInsightsIncidents(workspaceResourceId).GetAllAsync())
            {
                incidents.Add(MapIncident(incidentResource.Data));
            }

            return incidents;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Sentinel incidents.");

            throw;
        }
    }
    public async Task<SentinelIncident> CreateIncidentAsync(CreateIncidentRequest request)
    {
        try
        {
            var incidentId = Guid.NewGuid().ToString();

            var workspaceResourceId = new ResourceIdentifier(
                $"/subscriptions/{_options.SentinelSubscriptionId}" +
                $"/resourceGroups/{_options.ResourceGroupName}" +
                "/providers/Microsoft.OperationalInsights" +
                $"/workspaces/{_options.WorkspaceName}");

            var incidents = armClient.GetSecurityInsightsIncidents(workspaceResourceId);

            var severity = request.Severity.ToLowerInvariant() switch
            {
                "high" => SecurityInsightsIncidentSeverity.High,
                "medium" => SecurityInsightsIncidentSeverity.Medium,
                "low" => SecurityInsightsIncidentSeverity.Low,
                "informational" => SecurityInsightsIncidentSeverity.Informational,
                _ => throw new ArgumentException($"Unsupported incident severity '{request.Severity}'.", nameof(request))
            };

            var data = new SecurityInsightsIncidentData
            {
                Title = request.Title,
                Description = request.Description,
                Severity = severity,
                Status = SecurityInsightsIncidentStatus.New
            };

            var operation = await incidents.CreateOrUpdateAsync(WaitUntil.Completed, incidentId, data);

            return MapIncident(operation.Value.Data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Sentinel incident.");

            throw;
        }
    }
    private static SentinelIncident MapIncident(
        SecurityInsightsIncidentData data)
    {
        return new SentinelIncident
        {
            Id = data.Name,
            Title = data.Title,
            Severity = data.Severity.ToString(),
            Status = data.Status.ToString(),
            Description = data.Description,
            CreatedAt = data.CreatedOn?.UtcDateTime ?? DateTime.UtcNow,
            User = string.Empty,
            SourceIp = string.Empty
        };
    }
}