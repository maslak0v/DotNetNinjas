
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Helpers;

namespace Wallet.API.Helpers
{
    public static class ResponseCreator
    {
        public static ActionResult Create(OperationResult result)
            => result.StatusCode switch
        {
            Enum_StatusCode.NotFound => new NotFoundResult(),
            Enum_StatusCode.NoContent => new NoContentResult(),
            Enum_StatusCode.OK => new OkObjectResult(result.Message),
            Enum_StatusCode.BadRequest => new BadRequestObjectResult(result.Message),
            Enum_StatusCode.ServerError => new ObjectResult(result.Message) { StatusCode = (int)result.StatusCode},
            _ => throw new NotImplementedException("Uknown status code")
        };
}
}
