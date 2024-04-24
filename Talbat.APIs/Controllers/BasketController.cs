using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs.Controllers
{

	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
			_basketRepository = basketRepository;
			_mapper = mapper;
		}


		[HttpGet]// api/basket?id=

		public async Task<ActionResult<CustomerBasket>>GetBasket(string id)
		{
			var basket = await _basketRepository.GetBasketAsync(id);
			return Ok(basket ?? new CustomerBasket(id));
		}

		[HttpPost] //POST: /api/basket
		public async Task<ActionResult<CustomerBasket>>UpdateBasket(CustomerBasketDto basket)
		{
			var mappedbasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

			var CreatedOrUpdated =  await _basketRepository.UpdateBasketAsync(mappedbasket);
			if (CreatedOrUpdated is null)
				return BadRequest(new ApiResponse(400));

			return Ok(CreatedOrUpdated);
		}

		[HttpDelete] //Delete : /api/basket?id=

		public async Task DeleteBasket(string id)
		{
			 await _basketRepository.DeleteBasketAsync(id);
		}
    }
}
