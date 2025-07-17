namespace Primitives.Shared.DTOs;

public record ResponseDto(
    object? Result = null,
    bool IsSuccess = true,
    string? Message = null,
    ErrorDto? Error = null);