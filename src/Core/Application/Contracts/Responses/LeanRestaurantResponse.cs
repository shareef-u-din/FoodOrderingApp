using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Responses
{
    public class LeanRestaurantResponse
    {
        public Guid RestaurantId { get; set; }
        public string Name { get; set; }
    }
}
