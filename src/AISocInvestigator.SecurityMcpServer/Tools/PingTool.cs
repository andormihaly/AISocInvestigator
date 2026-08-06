using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AISocInvestigator.SecurityMcpServer.Tools.Ping;

[McpServerToolType]
public sealed class PingTool
{
    [McpServerTool]
    [Description("Checks whether the Security MCP Server is available.")]
    public object Ping()
    {
        return new
        {
            Status = "ok",
            Service = "AI SOC Investigator Security MCP Server",
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}