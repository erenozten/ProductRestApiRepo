using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Responses;

namespace ProductRestApi.SwaggerExamples.Common;

public class Swagger409Response : IExamplesProvider<GenericApiResponse<object>>
{
    public GenericApiResponse<object> GetExamples() => SwaggerErrorFactory.Create(StatusCodes.Status409Conflict);
}