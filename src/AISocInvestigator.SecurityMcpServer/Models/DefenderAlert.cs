namespace AISocInvestigator.SecurityMcpServer.Models;

public sealed record DefenderAlert(
    string Id,
    string Title,
    string Severity,
    string Status,
    string? Category,
    string? AlertWebUrl);