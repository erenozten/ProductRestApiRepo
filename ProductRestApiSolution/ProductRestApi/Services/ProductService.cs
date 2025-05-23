using AutoMapper;
using FluentValidation;
using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Helpers;
using ProductRestApi.Common.Logging;
using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs;
using ProductRestApi.DTOs.Product;
using ProductRestApi.Entities;
using ProductRestApi.Interfaces.Repositories;

namespace ProductRestApi.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductService> _logger;

    private readonly IValidator<ProductPostRequestDto> _postValidator;
    private readonly IValidator<ProductPutRequestDto> _putValidator;
    private readonly IValidator<ProductPatchRequestDto> _patchValidator;

    public ProductService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<ProductService> logger,
        IValidator<ProductPostRequestDto> postValidator,
        IValidator<ProductPutRequestDto> putValidator,
        IValidator<ProductPatchRequestDto> patchValidator)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _postValidator = postValidator;
        _putValidator = putValidator;
        _patchValidator = patchValidator;
    }

    public async Task<GenericApiResponse<ProductGetResponseDto>> GetProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning($"{{@{LoggingMessageTemplate.ProductLogModel}}} Product not found", new ProductLogModel { Id = id });
            return ApiResponseHelper.NotFound<ProductGetResponseDto>(id);
        }

        var productLogModel = _mapper.Map<ProductLogModel>(product);
        _logger.LogInformation(LoggingTemplates.ProductFound, productLogModel);
        return ApiResponseHelper.Success(_mapper.Map<ProductGetResponseDto>(product));
    }

    public async Task<GenericApiResponse<ListWithCountDto<ProductGetResponseDto>>> GetProducts()
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync();
        var productsDto = _mapper.Map<List<ProductGetResponseDto>>(products);
        var productsLogModel = _mapper.Map<List<ProductLogModel>>(productsDto);

        _logger.LogInformation(LoggingTemplates.ProductsFound, productsLogModel);
        return ApiResponseHelper.SuccessList(productsDto);
    }

    // public async Task<GenericApiResponse<object>> DeleteProduct(int id)
    public async Task<GenericApiResponse<object>> DeleteProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning(LoggingTemplates.ProductNotFoundError, new ProductLogModel { Id = id });
            return ApiResponseHelper.NotFound<object>(id);
        }

        var productLogModel = _mapper.Map<ProductLogModel>(product);

        bool isDeleted = await _unitOfWork.ProductRepository.DeleteAsync(id);
        if (!isDeleted)
        {
            _logger.LogWarning(LoggingTemplates.ProductFoundButDeletionError, productLogModel);
            return ApiResponseHelper.InternalError<object>();
        }

        _logger.LogInformation(LoggingTemplates.ProductDeletedSuccessfully, productLogModel);
        return ApiResponseHelper.SuccessWith204<object>();
    }

    public async Task<GenericApiResponse<ProductPutResponseDto>> UpdateProduct(ProductPutRequestDto dto, int id)
    {
        var productLogModel = _mapper.Map<ProductLogModel>(dto);
        productLogModel.Id = id;

        if (id <= 0)
        {
            _logger.LogWarning(LoggingTemplates.InvalidIdError, new ProductLogModel { Id = id });
            return ApiResponseHelper.BadRequest<ProductPutResponseDto>();
        }

        var validationResult = await _putValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(LoggingTemplates.ValidationError, productLogModel);
            return ApiResponseHelper.ValidationFail<ProductPutResponseDto>(validationResult.Errors);
        }

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning(LoggingTemplates.ProductNotFoundError, productLogModel);
            return ApiResponseHelper.NotFound<ProductPutResponseDto>(id);
        }

        bool duplicate = await _unitOfWork.ProductRepository.AnyAsync(x =>
            x.Id != id && x.Name!.ToLower() == dto.Name!.ToLower());

        if (duplicate)
        {
            _logger.LogWarning(LoggingTemplates.ProductNameDuplicateError, productLogModel);
            return ApiResponseHelper.Duplicate<ProductPutResponseDto>(dto.Name!);
        }

        _mapper.Map(dto, product);
        await _unitOfWork.SaveAsync();

        _logger.LogInformation(LoggingTemplates.ProductUpdated, productLogModel);
        return ApiResponseHelper.Success(_mapper.Map<ProductPutResponseDto>(product));
    }

    public async Task<GenericApiResponse<ProductPostResponseDto>> CreateProduct(ProductPostRequestDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        var productLogModel = _mapper.Map<ProductLogModel>(dto);

        var validationResult = await _postValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(LoggingTemplates.ValidationError, productLogModel);
            return ApiResponseHelper.ValidationFail<ProductPostResponseDto>(validationResult.Errors);
        }

        bool exists = await _unitOfWork.ProductRepository.AnyAsync(x => x.Name!.ToLower() == dto.Name!.ToLower());
        if (exists)
        {
            _logger.LogWarning(LoggingTemplates.ProductNameDuplicateError, productLogModel);
            return ApiResponseHelper.Duplicate<ProductPostResponseDto>(dto.Name!);
        }

        await _unitOfWork.ProductRepository.AddAsync(product);
        await _unitOfWork.SaveAsync();

        _logger.LogInformation(LoggingTemplates.ProductCreatedSuccessfully, _mapper.Map<ProductLogModel>(product));
        return ApiResponseHelper.Success(_mapper.Map<ProductPostResponseDto>(product));
    }

    public async Task<GenericApiResponse<ProductPatchResponseDto>> PatchProduct(ProductPatchRequestDto dto, int id)
    {
        var productLogModel = _mapper.Map<ProductLogModel>(dto);
        productLogModel.Id = id;

        if (id <= 0)
        {
            _logger.LogWarning(LoggingTemplates.InvalidIdError, new ProductLogModel { Id = id });
            return ApiResponseHelper.BadRequest<ProductPatchResponseDto>();
        }

        var validationResult = await _patchValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(LoggingTemplates.ValidationError, productLogModel);
            return ApiResponseHelper.ValidationFail<ProductPatchResponseDto>(validationResult.Errors);
        }

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning(LoggingTemplates.ProductNotFoundError, productLogModel);
            return ApiResponseHelper.NotFound<ProductPatchResponseDto>(id);
        }

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            bool duplicate = await _unitOfWork.ProductRepository.AnyAsync(x =>
                x.Id != id && x.Name!.ToLower() == dto.Name.ToLower());

            if (duplicate)
            {
                _logger.LogWarning(LoggingTemplates.ProductNameDuplicateError, productLogModel);
                return ApiResponseHelper.Duplicate<ProductPatchResponseDto>(dto.Name);
            }
        }

        _mapper.Map(dto, product);
        await _unitOfWork.SaveAsync();

        _logger.LogInformation(LoggingTemplates.ProductPatched, productLogModel);
        return ApiResponseHelper.Success(_mapper.Map<ProductPatchResponseDto>(product));
    }
}
