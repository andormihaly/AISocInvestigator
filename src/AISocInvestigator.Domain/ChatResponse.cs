namespace AISocInvestigator.Application.Models;

public sealed record ChatResponse(
    string Content,
    string ResponseId);