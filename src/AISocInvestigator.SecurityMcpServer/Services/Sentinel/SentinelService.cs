using AISocInvestigator.SecurityMcpServer.Models;

namespace AISocInvestigator.SecurityMcpServer.Services.Sentinel;

public sealed class SentinelService( ISentinelClient sentinelClient) : ISentinelService
{
    public async Task<IReadOnlyList<SentinelIncident>> GetIncidentsAsync()
    {
        return await sentinelClient.GetIncidentsAsync();
    }

    public async Task<SentinelIncident?> GetIncidentAsync(string id)
    {
        return await sentinelClient.GetIncidentAsync(id);
    }
}