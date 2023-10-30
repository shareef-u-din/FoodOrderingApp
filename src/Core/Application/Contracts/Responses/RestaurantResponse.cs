using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Responses
{
    public class RestaurantResponse
    {
        public Guid RestaurantId { get; set; }
        public string Name { get; set; }
        public RestaurantAddressResponse RestaurantAddress { get; set; }
        public Menu Menu { get; set; }
    }
}
