using AISocInvestigator.Domain.Incidents;

namespace AISocInvestigator.Functions.Clients;

public interface ISentinelClient
{
    Task<Incident?> GetIncidentAsync(string id);
    Task<IReadOnlyList<Incident>> GetIncidentsAsync();
}