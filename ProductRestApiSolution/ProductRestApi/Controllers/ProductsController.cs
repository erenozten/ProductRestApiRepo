using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductRestApi.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs;
using ProductRestApi.DTOs.Product;
using ProductRestApi.Interfaces.Repositories;
using ProductRestApi.SwaggerExamples.Common;
using ProductRestApi.SwaggerExamples.Product;

namespace ProductRestApi.Controllers;

[ApiController]
[Route("api/v1/Products")]
public class ProductsController : BaseController
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IProductService productService, IUnitOfWork unitOfWork)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id}")]
    #region 'Get' swagger examples and tag
    [Tags("Products (Query)")]
    [ProducesResponseType(typeof(GenericApiResponse<ProductGetResponseDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SwaggerGetProduct200Response))]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(Swagger404Response))]
    [SwaggerOperation(Summary = "Retrieves a product by its ID", Description = "Returns 200 OK with product details if found. Returns 404 Not Found if the product does not exist. Returns 400 Bad Request if the ID is invalid.")]
    #endregion
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var serviceResponse = await _productService.GetProduct(id);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpGet]
    #region 'Get' swagger examples and tag
    [Tags("Products (Query)")]
    [ProducesResponseType(typeof(GenericApiResponse<ListWithCountDto<ProductGetResponseDto>>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SwaggerGetProducts200Response))]
    [SwaggerOperation( Summary = "Retrieves all products", Description = "Returns 200 OK with a list of all products." )]
    #endregion
    public async Task<IActionResult> Get()
    {
        var serviceResponse = await _productService.GetProducts();
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpDelete("{id}")]
    #region 'Delete' swagger examples and tag
    [Tags("Products (Command)")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerResponseExample(StatusCodes.Status204NoContent, typeof(object))]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(Swagger404Response))]
    [SwaggerOperation( Summary = "Deletes a product by its ID", Description = "Returns 204 No Content if the product is successfully deleted. Returns 404 Not Found if the product does not exist. Returns 400 Bad Request if the ID is invalid." )]
    #endregion
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var serviceResponse = await _productService.DeleteProduct(id);
        if (serviceResponse.StatusCode == StatusCodes.Status204NoContent)
            return NoContent();

        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpPut("{id}")]
    #region 'Put' swagger examples and tag
    [Tags("Products (Command)")]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status409Conflict)]
    [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(Swagger409Response))]
    [ProducesResponseType(typeof(GenericApiResponse<ProductGetResponseDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SwaggerGetProduct200Response))]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(Swagger404Response))]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProductGenericValidationErrorResponse))]
    [SwaggerOperation(Summary = "Updates the product by ID", Description = "Returns 400 Bad Request if validation fails or input is invalid. Returns 404 if the product is not found.")]
    #endregion
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ProductPutRequestDto productRequestDto)
    {
        var serviceResponse = await _productService.UpdateProduct(productRequestDto, id);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpPost]
    #region 'Post' swagger examples and tag
    [Tags("Products (Command)")]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status409Conflict)]
    [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(Swagger409Response))]
    [ProducesResponseType(typeof(GenericApiResponse<ProductGetResponseDto>), StatusCodes.Status201Created)]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(SwaggerPostProduct201Response))]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProductGenericValidationErrorResponse))]
    [SwaggerOperation( Summary = "Creates a new product", Description = "Returns 201 Created with the newly created product. Returns 400 Bad Request if validation fails. Returns 409 Conflict if a product with the same name already exists." )]
    # endregion
    public async Task<IActionResult> Post([FromBody] ProductPostRequestDto productRequestDto)
    {
        var serviceResponse = await _productService.CreateProduct(productRequestDto);
        if (serviceResponse.Data == null)
            return StatusCode(serviceResponse.StatusCode, serviceResponse);

        return CreatedAtAction(
            nameof(Get),
            new { id = serviceResponse.Data.Id },
            GenericApiResponse<ProductPostResponseDto>.Success(
                serviceResponse.Data,
                StatusCodes.Status201Created
            ));
    }

    [HttpPatch("{id}")]
    #region 'Patch' swagger examples and tag
    [Tags("Products (Command)")]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status409Conflict)]
    [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(Swagger409Response))]
    [ProducesResponseType(typeof(GenericApiResponse<ProductGetResponseDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SwaggerGetProduct200Response))]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(Swagger404Response))]
    [ProducesResponseType(typeof(GenericApiResponse<object>), StatusCodes.Status400BadRequest)]
    [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ProductGenericValidationErrorResponse))]
    [SwaggerOperation( Summary = "Partially updates a product by its ID", Description = "Returns 200 OK with the updated product if successful. Returns 400 Bad Request if validation fails or the ID is invalid. Returns 404 Not Found if the product does not exist. Returns 409 Conflict if a product with the same name already exists." )]
    #endregion
    public async Task<IActionResult> Patch(int id, [FromBody] ProductPatchRequestDto patchDto)
    {
        var serviceResponse = await _productService.PatchProduct(patchDto, id);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }
}