using AISocInvestigator.Domain.Incidents;

namespace AISocInvestigator.Functions.Services;

public interface IIncidentService
{
    Task<Incident?> GetIncidentAsync(string id);
    Task<IReadOnlyList<Incident>> GetIncidentsAsync();
}