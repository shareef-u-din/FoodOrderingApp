using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Restaurant : BaseAuditableEntity
    {
        private Restaurant() { }
        public Guid RestaurantId { get; private set; }
        public string Name { get; private set; }
        public RestaurantAddress RestaurantAddress { get; private set; }
        public Menu Menu { get; private set; }

        public static Restaurant CreateRestaurant(Guid restaurantId, string Name)
        {
            return new Restaurant
            {
                RestaurantId = restaurantId,
                Name = Name,
                CreatedBy = string.Empty,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = string.Empty,
                UpdatedDate = DateTime.UtcNow
            };
        }

        public static Restaurant AddRestaurantAddress(Restaurant restaurant, RestaurantAddress restaurantAddress)
        {
            restaurant.RestaurantAddress = restaurantAddress;
            return restaurant;
        }

        public static Restaurant AddRestaurantMenu(Restaurant restaurant, Menu menu)
        {
            restaurant.Menu = menu;
            return restaurant;
        }

        public static Restaurant UpdateRestaurantName(Restaurant restaurant, string RestaurantName)
        {
            restaurant.Name = RestaurantName;
            return restaurant;
        }
    }
}
