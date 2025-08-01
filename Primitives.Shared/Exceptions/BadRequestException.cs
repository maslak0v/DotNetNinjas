
namespace Primitives.Shared.Exceptions
{
    public class BadRequestException(string error)
        : GlobalException(400, "Bad request", error)
    {
    }
}
