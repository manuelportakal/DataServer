using DataServer.Common.ResponseObjects;
using Microsoft.AspNetCore.Mvc;

namespace DataServer.Api.Utilities
{
    public class ResponseMapper
    {
        public static ActionResult Map<T>(CustomResponse<T> customResponse) where T : class
        {
            return customResponse.StatusCode switch
            {
                200 => new OkObjectResult(customResponse),
                401 => new UnauthorizedObjectResult(customResponse),
                500 => new ConflictObjectResult(customResponse),
                _ => new ConflictObjectResult(customResponse),
            };
        }
    }
}