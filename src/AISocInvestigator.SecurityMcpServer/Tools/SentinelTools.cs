using AISocInvestigator.SecurityMcpServer.Models;
using AISocInvestigator.SecurityMcpServer.Services.Sentinel;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AISocInvestigator.SecurityMcpServer.Tools.Sentinel;

[McpServerToolType]
public sealed class SentinelTools(ISentinelService sentinelService)
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
    [Description("Creates a new Microsoft Sentinel incident in the configured workspace.")]
    public async Task<SentinelIncident> CreateIncident([Description("The title of the incident.")] string title, [Description("A detailed description of the incident.")] string description, [Description("The severity of the incident. Allowed values: High, Medium, Low, Informational.")] string severity)
    {
        var request = new CreateIncidentRequest(title, description, severity);

        return await sentinelService.CreateIncidentAsync(request);
    }
}