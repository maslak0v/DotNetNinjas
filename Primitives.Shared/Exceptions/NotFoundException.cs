

namespace Primitives.Shared.Exceptions
{
    public class NotFoundException(string error) : GlobalException(404, "Not found", error)
    {}
}
