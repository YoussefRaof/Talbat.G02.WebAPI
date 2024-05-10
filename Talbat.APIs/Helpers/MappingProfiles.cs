using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
	public class MappingProfiles:Profile
	{
		private readonly IConfiguration _configuration;

		//public MappingProfiles(IConfiguration configuration)
  //      {
		//	_configuration = configuration;
  //          CreateMap<Product, ProductToReturnDto>()
  //                   .ForMember(D => D.Brand, O => O.MapFrom(S => S.Brand.Name))
  //                   .ForMember(D => D.Category, O => O.MapFrom(S => S.Category.Name))
  //                   .ForMember(D => D.PictureUrl, O => O.MapFrom(S => $"{_configuration["ApiBaseUrl"]}/{S.PictureUrl}");
		//}
		public MappingProfiles(string Url)
		{
			
			CreateMap<Product, ProductToReturnDto>()
					 .ForMember(D => D.Brand, O => O.MapFrom(S => S.Brand.Name))
					 .ForMember(D => D.Category, O => O.MapFrom(S => S.Category.Name))
					 .ForMember(D => D.PictureUrl, O => O.MapFrom(S => $"{Url}/{S.PictureUrl}"));

			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();
			CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();
			CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggregate.Address>();
		}
	}
}
