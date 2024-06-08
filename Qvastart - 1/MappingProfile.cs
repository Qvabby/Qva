using AutoMapper;
using Qvastart___1.Models;
using Qvastart___1.ViewModels;
using Qvastart___1.ViewModels.Dtos;

namespace Qvastart___1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<QvaUser, ProfileViewModel>().ReverseMap();
            CreateMap<ProductViewModel, Product>().ReverseMap();
            CreateMap<Product,GetProductDto>()
                .ForMember(dest => dest.UserPurchasedProducts, opt=> opt.MapFrom(src => src.UserPurchasedProducts))
                .ForMember(dest => dest.UserWishlistedProducts, opt => opt.MapFrom(src => src.UserWishlistedProducts))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ReverseMap();
            //CreateMap<List<GetProductDto>, List<Product>>().ReverseMap();
        }
    }
}
