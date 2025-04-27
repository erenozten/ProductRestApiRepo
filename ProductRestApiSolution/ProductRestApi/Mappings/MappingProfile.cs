using AutoMapper;
using ProductRestApi.DTOs;
using ProductRestApi.DTOs.Product;
using ProductRestApi.Entities;

namespace ProductRestApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region get product mappings
            CreateMap<Product, ProductGetResponseDto>().ReverseMap();
            #endregion

            #region create new product mappings
            CreateMap<ProductPostRequestDto, ProductPostResponseDto>().ReverseMap();
            CreateMap<Product, ProductPostRequestDto>().ReverseMap();
            CreateMap<Product, ProductPostResponseDto>().ReverseMap();
            #endregion

            #region update product mappings
            CreateMap<ProductPutRequestDto, ProductPutResponseDto>().ReverseMap();
            CreateMap<Product, ProductPutRequestDto>().ReverseMap();
            CreateMap<Product, ProductPutResponseDto>().ReverseMap();
            #endregion

            #region patch product mappings
            CreateMap<ProductPatchRequestDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, ProductPatchResponseDto>();
            CreateMap<ProductPatchRequestDto, ProductPatchResponseDto>();
            #endregion
        }
    }
}