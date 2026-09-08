using AISocInvestigator.SecurityMcpServer.Models;
using AISocInvestigator.SecurityMcpServer.Services.Alert;
using AISocInvestigator.SecurityMcpServer.Services.Sentinel;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AISocInvestigator.SecurityMcpServer.Tools.Sentinel;

[McpServerToolType]
public sealed class SentinelTools(ISentinelService sentinelService, IAlertService alertService)
{
    [McpServerTool]
    [Description("Lists all Microsoft Sentinel incidents from the configured workspace.")]
    public async Task<IReadOnlyList<SentinelIncident>> ListIncidents()
    {
        return await sentinelService.GetIncidentsAsync();
    }

    [McpServerTool]
    [Description("Retrieves a Microsoft Sentinel incident by its identifier.")]
    public async Task<SentinelIncident?> GetIncident([Description("The Microsoft Sentinel incident identifier.")] string incidentId)
    {
        return await sentinelService.GetIncidentAsync(incidentId);
    }

    [McpServerTool]
    [Description("Creates a new Microsoft Defender alert based on investigated security activity.")]
    public async Task<DefenderAlert> CreateAlert(
     [Description("The title of the alert.")] string title,
     [Description("A detailed description of the security activity.")] string description,
     [Description("The severity of the alert. Allowed values: High, Medium, Low, Informational.")] string severity,
     [Description("The Microsoft Defender alert category, for example InitialAccess.")] string category,
     [Description("The type of the impacted entity, for example Ip.")] string entityType,
     [Description("The identifier used for the impacted entity, for example address.")] string entityIdentifier,
     [Description("The value of the impacted entity, for example the IP address.")] string entityValue)
    {
        var request = new CreateAlertRequest(title, description, severity, category, entityType, entityIdentifier, entityValue);

        return await alertService.CreateAlertAsync(request);
    }
}