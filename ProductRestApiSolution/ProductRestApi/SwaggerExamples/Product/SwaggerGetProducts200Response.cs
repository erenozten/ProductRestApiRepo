using Microsoft.AspNetCore.Http.Features;
using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs;
using ProductRestApi.DTOs.Product;

namespace ProductRestApi.SwaggerExamples.Product
{
    public class SwaggerGetProducts200Response : IExamplesProvider<GenericApiResponse<ListWithCountDto<ProductGetResponseDto>>>
    {
        public GenericApiResponse<ListWithCountDto<ProductGetResponseDto>> GetExamples()
        {
            return GenericApiResponse<ListWithCountDto<ProductGetResponseDto>>.Success(
                statusCode: StatusCodes.Status200OK,
                data: new ListWithCountDto<ProductGetResponseDto>
                {
                    Items = new List<ProductGetResponseDto>
                    {
                        new ProductGetResponseDto { Id = 1, Name = "Cup", About = "Used for drinking liquids" },
                        new ProductGetResponseDto { Id = 2, Name = "Dagger", About = "A small, sharp weapon for close combat" },
                        new ProductGetResponseDto { Id = 3, Name = "Book", About = "Used for reading and gaining knowledge" }
                    },
                    TotalCount = 3
                },
                message: "");
        }
    }
}