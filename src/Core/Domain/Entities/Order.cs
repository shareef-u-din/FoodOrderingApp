using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseAuditableEntity
    {
        public Guid OrderId { get; private set; }
        public Guid Restaurant { get; private set; }
        public Customer Customer { get; private set; }
        public List<Menu> OrderItems { get; private set; }
        public int TotalAmount { get; private set; }
        public DateTime OrderTime { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }

        private Order() { }

        public static Order CreateOrder(Guid OrderId, Customer customer, List<Menu> orderitems,
            int totalAmount, OrderStatus orderStatus, PaymentStatus paymentStatus)
        {
            return new Order
            {
                CreatedBy = string.Empty,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = string.Empty,
                UpdatedDate = DateTime.UtcNow,
                OrderId = OrderId,
                OrderStatus = orderStatus,
                PaymentStatus = paymentStatus,
                Customer = customer,
                OrderItems = orderitems,
                TotalAmount = totalAmount,
                OrderTime = DateTime.UtcNow
            };
        }
    }
}
