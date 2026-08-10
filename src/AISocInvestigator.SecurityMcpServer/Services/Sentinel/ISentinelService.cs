using AISocInvestigator.SecurityMcpServer.Models;

namespace AISocInvestigator.SecurityMcpServer.Services.Sentinel;

public interface ISentinelService
{
    Task<IReadOnlyList<SentinelIncident>> GetIncidentsAsync();

    Task<SentinelIncident?> GetIncidentAsync(string id);
}