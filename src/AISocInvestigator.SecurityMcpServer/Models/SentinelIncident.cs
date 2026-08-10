namespace AISocInvestigator.SecurityMcpServer.Models;

public sealed class SentinelIncident
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required string Severity { get; init; }

    public required string Status { get; init; }

    public string? Description { get; init; }

    public DateTime CreatedAt { get; init; }

    public required string User { get; init; }

    public required string SourceIp { get; init; }
}