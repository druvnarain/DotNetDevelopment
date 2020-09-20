using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    //For automapper. must add in startup.cs
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Source is Product, map to destination ProductToReturnDto
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(destinationDto => destinationDto.ProductBrand, 
                    forDtoMember => forDtoMember.MapFrom(sourceEntity => sourceEntity.ProductBrand.Name))
                .ForMember(destinationDto => destinationDto.ProductType, 
                    forDtoMember => forDtoMember.MapFrom(sourceEntity => sourceEntity.ProductType.Name))
                .ForMember(destinationDto => destinationDto.PictureUrl,
                    forDtoMember => forDtoMember.MapFrom<ProductUrlResolver>());
                
        }
    }
}