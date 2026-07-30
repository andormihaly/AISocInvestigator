public sealed class AskChatRequest
{
    public required string Message { get; init; }

    public string? PreviousResponseId { get; init; }
}