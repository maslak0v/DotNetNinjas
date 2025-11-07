namespace Primitives.Shared.DTOs
{
    public record ErrorDto(
        int StatusCode,
        string Title,
        string Details,
        string TraceId);
}
