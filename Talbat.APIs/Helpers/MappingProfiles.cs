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

			CreateMap<Order, OrderToReturnDto>()
					.ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
					.ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost));

			CreateMap<OrderItem, OrderItemDto>()
					.ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
					.ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName))
					.ForMember(d => d.PictureUrl, O => O.MapFrom(s => $"{Url}/{s.Product.PictureUrl}"));
					//.ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
		}
	}
}
