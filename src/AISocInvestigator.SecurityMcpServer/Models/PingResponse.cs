namespace AISocInvestigator.SecurityMcpServer.Models;

public class PingResponse
{
    public string Status { get; init; } = default!;
    public string Service { get; init; } = default!;
    public DateTimeOffset Timestamp { get; init; }
}