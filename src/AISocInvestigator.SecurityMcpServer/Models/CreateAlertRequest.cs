namespace AISocInvestigator.SecurityMcpServer.Models;

public sealed record CreateAlertRequest(
    string Title,
    string Description,
    string Severity,
    string Category,
    string EntityType,
    string EntityIdentifier,
    string EntityValue);