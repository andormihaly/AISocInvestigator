using AISocInvestigator.SecurityMcpServer.Models;

namespace AISocInvestigator.SecurityMcpServer.Services.Alert;

public interface IAlertService
{
    Task<DefenderAlert> CreateAlertAsync(CreateAlertRequest request);
}