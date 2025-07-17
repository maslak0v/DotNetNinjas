
namespace Primitives.Shared.Exceptions
{
    public class GlobalException(int statusCode, string title, string error)
        : Exception(error)
    {
        public int StatusCode { get; init; } = statusCode;
        public string Title { get; init; } = title;
    }
}
