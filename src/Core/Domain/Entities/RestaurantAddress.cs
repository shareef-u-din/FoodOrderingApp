using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RestaurantAddress : BaseAuditableEntity
    {
        public Guid RestaurantAddressId { get; private set; }
        public Guid RestaurantId { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }

        private RestaurantAddress() { }

        public static RestaurantAddress CreateAddress(Guid restaurantId, Guid restaurantAddressId, string street, string city, string state, string zipCode)
        {
            return new RestaurantAddress
            {
                RestaurantId = restaurantId,
                RestaurantAddressId = restaurantAddressId,
                CreatedBy = string.Empty,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = string.Empty,
                UpdatedDate = DateTime.UtcNow,
                Street = street,
                City = city,
                State = state,
                ZipCode = zipCode
            };
        }
    }
}
