using AISocInvestigator.Domain.Incidents;
using AISocInvestigator.Functions.Clients;

namespace AISocInvestigator.Functions.Services;

public class IncidentService(ISentinelClient sentinelClient) : IIncidentService
{
    public async Task<Incident?> GetIncidentAsync(string id)
    {
        return await sentinelClient.GetIncidentAsync(id);
    }

    public async Task<IReadOnlyList<Incident>> GetIncidentsAsync()
    {
        return await sentinelClient.GetIncidentsAsync();
    }
}