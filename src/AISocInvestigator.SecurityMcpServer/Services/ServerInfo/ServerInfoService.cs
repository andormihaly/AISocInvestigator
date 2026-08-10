using AISocInvestigator.SecurityMcpServer.Models;

namespace AISocInvestigator.SecurityMcpServer.Services.ServerInfo;

public sealed class ServerInfoService : IServerInfoService
{
    public PingResponse GetStatus()
    {
        return new PingResponse
        {
            Status = "ok",
            Service = "AI SOC Investigator Security MCP Server",
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}