using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Responses;

namespace ProductRestApi.SwaggerExamples.Common;

public class Swagger403Response : IExamplesProvider<GenericApiResponse<object>>
{
    public GenericApiResponse<object> GetExamples() => SwaggerErrorFactory.Create(StatusCodes.Status403Forbidden);
}
