namespace AISocInvestigator.Domain.Incidents;

public sealed class Incident
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Severity { get; init; }
    public required string Status { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required string User { get; init; }
    public required string SourceIp { get; init; }
    public required string Description { get; init; }
}