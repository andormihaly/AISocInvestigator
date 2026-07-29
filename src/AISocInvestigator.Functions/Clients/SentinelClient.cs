using AISocInvestigator.Domain.Incidents;
using AISocInvestigator.Functions.Authentication;
using AISocInvestigator.Functions.Configuration;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.SecurityInsights;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace AISocInvestigator.Functions.Clients;

public class SentinelClient(IHttpClientFactory httpClientFactory, IAccessTokenProvider accessTokenProvider, IOptions<SentinelOptions> sentinelOptions) : ISentinelClient
{
    public async Task<Incident?> GetIncidentAsync(string id)
    {
        var resourceId =
            SecurityInsightsIncidentResource.CreateResourceIdentifier(
                sentinelOptions.Value.SentinelSubscriptionId,
                sentinelOptions.Value.ResourceGroupName,
                sentinelOptions.Value.WorkspaceName,
                id);

        var armClient = new ArmClient(new DefaultAzureCredential());

        var incidentResource = armClient.GetSecurityInsightsIncidentResource(resourceId);

        var response = await incidentResource.GetAsync();

        var sentinelIncident = response.Value.Data;

        return new Incident
        {
            Id = sentinelIncident.Name,
            Title = sentinelIncident.Title,
            Severity = sentinelIncident.Severity.ToString(),
            Status = sentinelIncident.Status.ToString(),
            Description = sentinelIncident.Description,
            CreatedAt = sentinelIncident.CreatedOn?.UtcDateTime ?? DateTime.UtcNow,
            User = string.Empty,
            SourceIp = string.Empty
        };
    }

    public async Task<IReadOnlyList<Incident>> GetIncidentsAsync()
    {
        var workspaceResourceId = new ResourceIdentifier(
         $"/subscriptions/{sentinelOptions.Value.SentinelSubscriptionId}" +
         $"/resourceGroups/{sentinelOptions.Value.ResourceGroupName}" +
         "/providers/Microsoft.OperationalInsights" +
         $"/workspaces/{sentinelOptions.Value.WorkspaceName}");

        var armClient = new ArmClient(new DefaultAzureCredential());

        var sentinelWorkspace =
            armClient.GetOperationalInsightsWorkspaceSecurityInsightsResource(
                workspaceResourceId);

        var incidents = new List<Incident>();

        await foreach (var incidentResource in
            sentinelWorkspace.GetSecurityInsightsIncidents().GetAllAsync())
        {
            var data = incidentResource.Data;

            incidents.Add(new Incident
            {
                Id = data.Name,
                Title = data.Title,
                Severity = data.Severity.ToString(),
                Status = data.Status.ToString(),
                Description = data.Description,
                CreatedAt = data.CreatedOn?.UtcDateTime ?? DateTime.UtcNow,
                User = string.Empty,
                SourceIp = string.Empty
            });
        }

        return incidents;

    }
}