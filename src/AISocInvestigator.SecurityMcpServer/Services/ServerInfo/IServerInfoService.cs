using AISocInvestigator.SecurityMcpServer.Models;

namespace AISocInvestigator.SecurityMcpServer.Services.ServerInfo;

public interface IServerInfoService
{
    PingResponse GetStatus();
}