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
	[ApiExplorerSettings(IgnoreApi = true)]
	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(IOrderService orderService, IMapper mapper)
		{
			_orderService = orderService;
			_mapper = mapper;
		}


		[Authorize(AuthenticationSchemes = "Bearer")]

		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		// To Ignore Swagger Documentation 
		//POST  :  /api/Orders
		[HttpPost]

		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{

			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

			var email = User.FindFirstValue(ClaimTypes.Email);

			var order = await _orderService.CreateOrderAsync(orderDto.BasketId, address, email, orderDto.DeliveryMethodId);

			if (order is null)
				return BadRequest(new ApiResponse(400));

			return Ok(order);
		}

		[HttpGet]
		[Authorize(AuthenticationSchemes = "Bearer")]
		[ApiExplorerSettings(IgnoreApi = true)] // To Ignore Swagger Documentation 

		public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);

			var orders = await _orderService.GetOrdersForUserAsync(email);



			

			return Ok(orders);

		}
		[Authorize(AuthenticationSchemes = "Bearer")]
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")] //GET /api/Orders/1

		public async Task<ActionResult<Order>> GetOrderForUser(int id)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var order =  await _orderService.GetOrderByIdForUserAsync(id, email);

			if (order is null)
				return NotFound(new ApiResponse(404));

			return Ok(order);
		}

	}
}
