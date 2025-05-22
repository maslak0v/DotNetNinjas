namespace FinancialTracker.Services.AuthorizeApi.Domain.ValueObjects
{
    public enum Enum_StatusCode
    {
        OK = 200,
        CREATED = 201,
        NO_CONTENT = 204,
        BAD_REQUEST = 400,
        UNAUTHORIZED = 401,
        FORBIDDEN = 403,
        NOT_FOUND = 404,
        INVALID_TOKEN = 498,
        INTERNAL_SERVER_ERROR = 500,
        SERVICE_UNAVAILABLE = 503
    }
}
