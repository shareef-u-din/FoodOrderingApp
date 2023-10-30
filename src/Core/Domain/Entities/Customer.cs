using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Customer : BaseAuditableEntity
    {
        public Guid CustomerId { get; private set; }
        public string IdentityId { get; private set; }
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public bool IsRestaurantOwner { get; private set; }
        public CustomerLocation CustomerLocation { get; private set; }
        private Customer() { }

        public static Customer CreateCustomer(Guid customerId, string identityId, string firstName,
            string middleName, string lastName, string email, string phone, bool isRestaurantOwner)
        {
            return new Customer()
            {
                CustomerId = customerId,
                IdentityId = identityId,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                CreatedBy = string.Empty,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = string.Empty,
                UpdatedDate = DateTime.UtcNow,
                IsRestaurantOwner = isRestaurantOwner
            };
        }
    }
}
