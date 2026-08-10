using AISocInvestigator.SecurityMcpServer.Services.ServerInfo;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AISocInvestigator.SecurityMcpServer.Tools.Ping;

[McpServerToolType]
public sealed class PingTool(IServerInfoService serverInfoService)
{
    [McpServerTool]
    [Description("Checks whether the Security MCP Server is available.")]
    public object Ping()
    {
        return serverInfoService.GetStatus();
    }
}