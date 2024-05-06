﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
	public interface IOrderService
	{
		Task<Order>CreateOrderAsync(string basketId, Address shippingAddress ,string buyerEmail,int deliveryMethodId);
		Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

		Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail);

		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
	}
}
