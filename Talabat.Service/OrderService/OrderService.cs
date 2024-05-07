﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.UnitOfWork.Contract;

namespace Talabat.Service.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;

		//private readonly IGenericRepository<Product> _productRepo;
		//private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
		//private readonly IGenericRepository<Order> _orderRepo;

		public OrderService(
			IBasketRepository basketRepo , 
			IUnitOfWork unitOfWork)
        {
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
		}
        public async Task<Order?> CreateOrderAsync(string basketId, Address shippingAddress, string buyerEmail, int deliveryMethodId)
		{
			// 1.Get Basket From Baskets Repo
			var basket = await _basketRepo.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From Products Repo

			var orderItems = new List<OrderItem>();
			if(basket?.Items?.Count>0)
			{
				foreach(var item in basket.Items)
				{
					var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
					var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
					var orderItem = new OrderItem(productItemOrdered,product.Price,item.Quantity);

					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal
			var subtotal = orderItems.Sum( item => item.Price * item.Quantity);


			// 4. Get Delivery Method From DeliveryMethods Repo
			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

			// 5. Create Order
			var order = new Order(
				buyerEmail: buyerEmail,
				shippingAddress: shippingAddress,
				deliveryMethodId: deliveryMethodId,
				items: orderItems,
				subTotal: subtotal
				);

			 _unitOfWork.Repository<Order>().Add(order);


			// 6. Save To Database [TODO]
			var result = await _unitOfWork.CompleteAsync();
			if (result <= 0)
				return null;

			return order;



		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
