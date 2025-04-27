using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs;
using ProductRestApi.DTOs.Product;

namespace ProductRestApi.SwaggerExamples.Product
{
    public class SwaggerGetProduct200Response : IExamplesProvider<GenericApiResponse<ProductGetResponseDto>>
    {
        public GenericApiResponse<ProductGetResponseDto> GetExamples()
        {
            return GenericApiResponse<ProductGetResponseDto>.Success(
                statusCode: StatusCodes.Status200OK,
                data: new ProductGetResponseDto
                {
                    Id = 13,
                    Name = "Paris",
                    About = "A wonderful product"
                },
                message: "");
        }
    }
}