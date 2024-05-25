using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpec;

namespace Talabat.APIs.Controllers
{
	[ApiExplorerSettings(IgnoreApi = true)] // To Ignore Swagger Documentation 

	[Authorize(AuthenticationSchemes = "Bearer")]

	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(IOrderService orderService, IMapper mapper)
		{
			_orderService = orderService;
			_mapper = mapper;
		}



		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		// To Ignore Swagger Documentation 
		//POST  :  /api/Orders
		[HttpPost]

		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{

			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

			var email = User.FindFirstValue(ClaimTypes.Email);

			var order = await _orderService.CreateOrderAsync(orderDto.BasketId, address, email, orderDto.DeliveryMethodId);

			if (order is null)
				return BadRequest(new ApiResponse(400));

			return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
		}

		[HttpGet]
		

		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var orders = await _orderService.GetOrdersForUserAsync(email);



			

			return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(orders));

		}
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")] //GET /api/Orders/1

		public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var order =  await _orderService.GetOrderByIdForUserAsync(id, email);

			if (order is null)
				return NotFound(new ApiResponse(404));

			return Ok(_mapper.Map<OrderToReturnDto>(order));
		}

		[HttpGet("deliveryMethods")] //GET /api/Orders/deliveryMethods

		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var deliveryMethods = await _orderService.GetDeliveryMethodsAsync();

			return Ok(deliveryMethods);
		}


	}
}
