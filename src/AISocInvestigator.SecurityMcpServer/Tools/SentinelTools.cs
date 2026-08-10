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
}