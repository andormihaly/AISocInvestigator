using AISocInvestigator.SecurityMcpServer.Models;

namespace AISocInvestigator.SecurityMcpServer.Services.Sentinel;

public interface ISentinelClient
{
    Task<SentinelIncident?> GetIncidentAsync(string id);

    Task<IReadOnlyList<SentinelIncident>> GetIncidentsAsync();
}