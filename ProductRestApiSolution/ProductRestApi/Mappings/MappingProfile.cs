using AutoMapper;
using ProductRestApi.DTOs.Product;
using ProductRestApi.Entities;
using ProductRestApi.Common.Logging;

namespace ProductRestApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region get product mappings
            CreateMap<Product, ProductGetResponseDto>().ReverseMap();
            CreateMap<ProductGetResponseDto, ProductLogModel>().ReverseMap();
            CreateMap<Product, ProductLogModel>().ReverseMap();
            #endregion

            #region create new product mappings
            CreateMap<ProductPostRequestDto, ProductPostResponseDto>().ReverseMap();
            CreateMap<Product, ProductPostRequestDto>().ReverseMap();
            CreateMap<Product, ProductPostResponseDto>().ReverseMap();
            CreateMap<ProductPostRequestDto, ProductLogModel>().ReverseMap();
            CreateMap<ProductPostResponseDto, ProductLogModel>().ReverseMap();
            #endregion

            #region update product mappings
            CreateMap<ProductPutRequestDto, ProductPutResponseDto>().ReverseMap();
            CreateMap<Product, ProductPutRequestDto>().ReverseMap();
            CreateMap<Product, ProductPutResponseDto>().ReverseMap();
            CreateMap<ProductPutRequestDto, ProductLogModel>().ReverseMap();
            CreateMap<ProductPutResponseDto, ProductLogModel>().ReverseMap();
            #endregion

            #region patch product mappings
            CreateMap<ProductPatchRequestDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, ProductPatchResponseDto>();
            CreateMap<ProductPatchRequestDto, ProductPatchResponseDto>();
            CreateMap<ProductPatchRequestDto, ProductLogModel>().ReverseMap();
            CreateMap<ProductPatchResponseDto, ProductLogModel>().ReverseMap();
            #endregion
        }
    }
}