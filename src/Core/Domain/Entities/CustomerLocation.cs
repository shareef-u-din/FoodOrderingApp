using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CustomerLocation
    {
        public Guid CustomerId { get; private set; }
        public Guid CustomerLocationId { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        private CustomerLocation() { }

        public static CustomerLocation CreateCustomerAddress(Guid customerLocationId,Guid customerId, string street, string city, string state, string zipCode)
        {
            return new CustomerLocation
            {
                CustomerLocationId = customerLocationId,
                CustomerId = customerId,
                Street = street,
                City = city,
                State = state,
                ZipCode = zipCode
            };
        }
    }
}
