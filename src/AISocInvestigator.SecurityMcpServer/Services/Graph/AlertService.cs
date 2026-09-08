using AISocInvestigator.SecurityMcpServer.Models;
using AISocInvestigator.SecurityMcpServer.Services.Graph;

namespace AISocInvestigator.SecurityMcpServer.Services.Alert;

public sealed class AlertService(IGraphClient graphClient) : IAlertService
{
    public async Task<DefenderAlert> CreateAlertAsync(CreateAlertRequest request)
    {
        return await graphClient.CreateAlertAsync(request);
    }
}