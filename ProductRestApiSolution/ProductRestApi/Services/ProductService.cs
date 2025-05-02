using AutoMapper;
using FluentValidation.Results;
using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Extensions;
using ProductRestApi.Common.Helpers;
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
            _logger.LogWarning("{@MyProduct} Ürün bulunamadı.", new
                {
                    Product = new Product()
                    {
                        Id = id
                    }
                }
            );

            return ApiResponseHelper.NotFound<ProductGetResponseDto>(id);
        }

        _logger.LogInformation("Ürün başarıyla getirildi: Ürün bilgilieri sağdaki gibidir: {@MyProduct} ", new
            { MyProductId = product.Id, MyProductName = product.Name, MyProductAbout = product.About }
        );
        return ApiResponseHelper.Success(_mapper.Map<ProductGetResponseDto>(product));
    }

    public async Task<GenericApiResponse<ListWithCountDto<ProductGetResponseDto>>> GetProducts()
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync();
        var dtoList = _mapper.Map<List<ProductGetResponseDto>>(products);

        _logger.LogInformation("Toplam {Count} ürün getirildi.", dtoList.Count);
        return ApiResponseHelper.SuccessList(dtoList);
    }

    public async Task<GenericApiResponse<object>> DeleteProduct(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Ürün bulunamadı. ID: {Id}", id);
            return ApiResponseHelper.NotFound<object>(id);
        }

        bool isDeleted = await _unitOfWork.ProductRepository.DeleteAsync(id);
        if (!isDeleted)
        {
            _logger.LogWarning("{ProductInfo} Ürün bulundu ama silinemedi. ID: {Id}", id);
            return ApiResponseHelper.Fail<object>(StatusCodes.Status500InternalServerError,
                ConstMessages.DELETE_FAILED_Description,
                ConstMessages.DELETE_FAILED);
        }

        _logger.LogInformation("Ürün başarıyla silindi. ID: {Id}", id);
        return GenericApiResponse<object>.Success(null, StatusCodes.Status204NoContent);
    }

    public async Task<GenericApiResponse<ProductPutResponseDto>> UpdateProduct(ProductPutRequestDto dto, int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Geçersiz ID ile update request. ID: {Id}", id);
            return ApiResponseHelper.BadRequest<ProductPutResponseDto>();
        }

        var validator = new ProductPutRequestDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Update request validation hatası.");
            return ApiResponseHelper.ValidationFail<ProductPutResponseDto>(validationResult.Errors);
        }

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Güncellenmek istenen ürün bulunamadı. ID: {Id}", id);
            return ApiResponseHelper.NotFound<ProductPutResponseDto>(id);
        }

        bool duplicate = await _unitOfWork.ProductRepository.AnyAsync(x =>
            x.Id != id && x.Name!.ToLower() == dto.Name!.ToLower());

        if (duplicate)
        {
            _logger.LogWarning("Aynı isimde başka bir ürün mevcut. Name: {Name}", dto.Name);
            return ApiResponseHelper.Duplicate<ProductPutResponseDto>(dto.Name!);
        }

        _mapper.Map(dto, product);

        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Ürün başarıyla güncellendi. ID: {Id}, Name: {Name}", product.Id, product.Name);
        return ApiResponseHelper.Success(_mapper.Map<ProductPutResponseDto>(product));
    }

    public async Task<GenericApiResponse<ProductPostResponseDto>> CreateProduct(ProductPostRequestDto dto)
    {
        var validator = new ProductPostRequestDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation hataları bulundu.");
            return ApiResponseHelper.ValidationFail<ProductPostResponseDto>(validationResult.Errors);
        }

        bool exists = await _unitOfWork.ProductRepository.AnyAsync(x => x.Name!.ToLower() == dto.Name!.ToLower());
        if (exists)
        {
            _logger.LogWarning("Aynı isimde ürün mevcut. Name: {Name}", dto.Name!);
            return ApiResponseHelper.Duplicate<ProductPostResponseDto>(dto.Name!);
        }

        var product = _mapper.Map<Product>(dto);
        await _unitOfWork.ProductRepository.AddAsync(product);
        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Yeni ürün oluşturuldu. ID: {Id}, Name: {Name}", product.Id, product.Name);
        return ApiResponseHelper.Success(_mapper.Map<ProductPostResponseDto>(product));
    }

    public async Task<GenericApiResponse<ProductPatchResponseDto>> PatchProduct(ProductPatchRequestDto dto, int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Geçersiz ID ile patch request. ID: {Id}", id);
            return ApiResponseHelper.InvalidParameter<ProductPatchResponseDto>();
            // return BadRequest<ProductPatchResponseDto>();
        }

        var validator = new ProductPatchRequestDtoValidator();
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Patch request validation hatası.");
            return ApiResponseHelper.ValidationFail<ProductPatchResponseDto>(validationResult.Errors);
        }

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Patch yapılmak istenen ürün bulunamadı. ID: {Id}", id);
            return ApiResponseHelper.NotFound<ProductPatchResponseDto>(id);
        }

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            bool duplicate = await _unitOfWork.ProductRepository.AnyAsync(x =>
                x.Id != id && x.Name!.ToLower() == dto.Name.ToLower());

            if (duplicate)
            {
                _logger.LogWarning("Patch işleminde çakışan isim bulundu. Name: {Name}", dto.Name);
                return ApiResponseHelper.Duplicate<ProductPatchResponseDto>(dto.Name);
            }
        }

        _mapper.Map(dto, product);
        await _unitOfWork.SaveAsync();

        _logger.LogInformation("Ürün başarıyla patch'lendi. ID: {Id}, Name: {Name}", product.Id, product.Name);
        return ApiResponseHelper.Success(_mapper.Map<ProductPatchResponseDto>(product));
    }
}