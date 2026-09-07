namespace AISocInvestigator.SecurityMcpServer.Models;

public sealed record CreateIncidentRequest(string Title, string Description, string Severity);