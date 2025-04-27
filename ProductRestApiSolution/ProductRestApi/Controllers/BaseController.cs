using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Responses;
using ProductRestApi.SwaggerExamples.Common;

namespace ProductRestApi.Controllers
{
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(Swagger500Response))]

    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(Swagger401Response))]

    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status403Forbidden)]
    [SwaggerResponseExample(StatusCodes.Status403Forbidden, typeof(Swagger403Response))]
    public abstract class BaseController : ControllerBase
    {
    }
}