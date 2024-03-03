using AutoMapper;
using SMSHub.APIs.DTOs;
using SMSHub.Core.Entities;
using SMSHub.Core.Entities.Identity;

namespace SMSHub.APIs.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles() {
            //CreateMap <Product,ProductToReturnDto> ()
            //    //              destination                 source
            //    .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))//map from producttype.name inside product into producttype in ProductToReturnDto
            //    .ForMember(d=>d.ProductBrand,o=>o.MapFrom (s=>s.ProductBrand.Name))
            //    .ForMember(p=>p.PictureUrl,o=>o.MapFrom<ProductPictureUrlResolver>())
            //    ;

            CreateMap<AppUser, UserDto>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CreateSenderIdDto, SenderId>().ReverseMap();
            CreateMap<UpdateSenderIdDto, SenderId>().ReverseMap();
            //CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            //CreateMap<BasketItemDto, BasketItem>().ReverseMap();
        }

    }
}
