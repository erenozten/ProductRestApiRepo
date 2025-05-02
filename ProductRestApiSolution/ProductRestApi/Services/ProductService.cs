using AutoMapper;
using FluentValidation.Results;
using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Extensions;
using ProductRestApi.Common.Helpers;
using ProductRestApi.Common.Logging;
using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs;
using ProductRestApi.DTOs.Product;
using ProductRestApi.Entities;
using ProductRestApi.Interfaces.Repositories;
using ProductRestApi.Validators;

namespace ProductRestApi.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<ProductService> logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<GenericApiResponse<ProductGetResponseDto>> GetProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning($"{{@{LoggingMessageTemplate.ProductLogModel}}} Product not found", 
                new  ProductLogModel  { Id = id } 
            );
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

    // done
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
            return ApiResponseHelper.Fail<object>(
                StatusCodes.Status500InternalServerError,
                ConstMessages.DELETE_FAILED_Description,
                ConstMessages.DELETE_FAILED);
        }

        _logger.LogInformation(LoggingTemplates.ProductDeletedSuccessfully, productLogModel);
        return GenericApiResponse<object>.Success(null, StatusCodes.Status204NoContent);
    }

    // done
    public async Task<GenericApiResponse<ProductPutResponseDto>> UpdateProduct(ProductPutRequestDto dto, int id)
    {
        var productLogModel = _mapper.Map<ProductLogModel>(dto);
        productLogModel.Id = id;

        if (id <= 0)
        {
            _logger.LogWarning(LoggingTemplates.InvalidIdError, new ProductLogModel{Id = id});
            return ApiResponseHelper.BadRequest<ProductPutResponseDto>();
        }

        var validator = new ProductPutRequestDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);

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

    // done
    public async Task<GenericApiResponse<ProductPostResponseDto>> CreateProduct(ProductPostRequestDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        var productLogModel = _mapper.Map<ProductLogModel>(dto);

        var validator = new ProductPostRequestDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);

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

        var validator = new ProductPatchRequestDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);
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