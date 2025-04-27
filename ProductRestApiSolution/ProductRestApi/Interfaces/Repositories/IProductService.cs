using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs;
using ProductRestApi.DTOs.Product;

namespace ProductRestApi.Interfaces.Repositories
{
    public interface IProductService
    {
        Task<GenericApiResponse<ProductGetResponseDto>> GetProduct(int id);
        Task<GenericApiResponse<ListWithCountDto<ProductGetResponseDto>>> GetProducts();
        Task<GenericApiResponse<object>> DeleteProduct(int id);
        Task<GenericApiResponse<ProductPutResponseDto>> UpdateProduct(ProductPutRequestDto product, int id);
        Task<GenericApiResponse<ProductPostResponseDto>> CreateProduct(ProductPostRequestDto productDto);
        Task<GenericApiResponse<ProductPatchResponseDto>> PatchProduct(ProductPatchRequestDto productDto, int id);
    }
}