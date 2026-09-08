using AISocInvestigator.SecurityMcpServer.Models;
using AISocInvestigator.SecurityMcpServer.Services.Graph;

namespace AISocInvestigator.SecurityMcpServer.Services.Alert;

public sealed class AlertService(IGraphClient graphClient) : IAlertService
{
    public async Task<DefenderAlert> CreateAlertAsync(CreateAlertRequest request)
    {
        return await graphClient.CreateAlertAsync(request);
    }

    public async Task<IReadOnlyList<DefenderAlert>> GetAlertsAsync(DateTimeOffset? from = null)
    {
        return await graphClient.GetAlertsAsync(from);
    }

    public async Task<DefenderAlert?> GetAlertAsync(string alertId)
    {
        return await graphClient.GetAlertAsync(alertId);
    }
}