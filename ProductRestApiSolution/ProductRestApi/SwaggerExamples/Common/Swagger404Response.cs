using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Extensions;
using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs.Product;

namespace ProductRestApi.SwaggerExamples.Common
{
    public class Swagger404Response : IExamplesProvider<GenericApiResponse<object>>
    {
        public GenericApiResponse<object> GetExamples() => SwaggerErrorFactory.Create(StatusCodes.Status404NotFound);
    }
}