using AISocInvestigator.SecurityMcpServer.Models;

namespace AISocInvestigator.SecurityMcpServer.Services.Graph;

public interface IGraphClient
{
    Task<DefenderAlert> CreateAlertAsync(CreateAlertRequest request);
    Task<IReadOnlyList<DefenderAlert>> GetAlertsAsync();
    Task<DefenderAlert?> GetAlertAsync(string alertId);
}