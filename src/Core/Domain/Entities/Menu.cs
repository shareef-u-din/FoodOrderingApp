using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Menu : BaseAuditableEntity
    {
        private Menu() { }
        public Guid MenuId { get; private set; }
        public Guid RestaurantId { get; private set; }
        public string Title { get; private set; }
        public List<MenuCategory> MenuCategories { get; private set; }

        public static Menu CreateMenuItem(Guid restaurantId, Guid menuId, string title)
        {
            return new Menu
            {
                RestaurantId = restaurantId,
                MenuId = menuId,
                Title = title,
                CreatedBy = string.Empty,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = string.Empty,
                UpdatedDate = DateTime.UtcNow
            };
        }

        public static Menu UpdateMenuTitle(Menu menu, string title)
        {
            menu.Title = title;
            return menu;
        }
    }
}
