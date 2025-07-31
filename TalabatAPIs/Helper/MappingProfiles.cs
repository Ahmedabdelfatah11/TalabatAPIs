using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Cart;
using TalabatAPIs.Dtos;

namespace TalabatAPIs.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>().
                ForMember(d=>d.Brand, o => o.MapFrom(s => s.Brand.Name)).
                ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name)).
                ForMember(P=>P.PictureUrl,O=>O.MapFrom<ProductPictureUrlResolver>());
            CreateMap<CustomerCartDto, CustomerCart>();
            CreateMap<CartItemDto, CartItem>();


            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
