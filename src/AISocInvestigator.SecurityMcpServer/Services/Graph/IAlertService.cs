using AISocInvestigator.SecurityMcpServer.Models;

namespace AISocInvestigator.SecurityMcpServer.Services.Alert;

public interface IAlertService
{
    Task<DefenderAlert> CreateAlertAsync(CreateAlertRequest request);
    Task<IReadOnlyList<DefenderAlert>> GetAlertsAsync(DateTimeOffset? from = null);
    Task<DefenderAlert?> GetAlertAsync(string alertId);
}