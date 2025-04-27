using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs.Product;

namespace ProductRestApi.SwaggerExamples.Common
{
    public class Swagger500Response : IExamplesProvider<GenericApiResponse<object>>
    {
        public GenericApiResponse<object> GetExamples() => SwaggerErrorFactory.Create(StatusCodes.Status500InternalServerError);
    }
}