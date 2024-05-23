using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs
{
	public class OrderToReturnDto
	{
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = null!;

		public DateTimeOffset OrderDate { get; set; } 

		public string Status { get; set; } 

		public Address ShippingAddress { get; set; } = null!;

        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public virtual ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>(); // Navigational Property [Many]

		public decimal SubTotal { get; set; } // Total Without Delivery Cost

        public decimal Total { get; set; }


        public string PaymentIntentId { get; set; } = string.Empty;

	}
}
