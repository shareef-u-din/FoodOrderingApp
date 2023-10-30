using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MenuCategory : BaseAuditableEntity
    {
        public Guid MenuId { get; private set; }
        public Guid MenuCategoryId { get; private set; }
        public string Title { get; private set; }
        public List<Cuisine> Cuisines { get; private set; }

        private MenuCategory() { }

        public static MenuCategory CreateMenuCategory(Guid menuId, Guid menuCategoryId, string title)
        {
            return new MenuCategory
            {
                CreatedBy = string.Empty,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = string.Empty,
                UpdatedDate = DateTime.UtcNow,
                MenuId = menuId,
                MenuCategoryId = menuCategoryId,
                Title = title
            };
        }
    }
}
