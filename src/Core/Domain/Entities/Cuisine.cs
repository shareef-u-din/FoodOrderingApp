using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cuisine : BaseAuditableEntity
    {
        public Guid CuisineId { get; private set; }
        public Guid MenuCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }
        private Cuisine() { }

        public static Cuisine CreateCuisine(Guid cuisineId, Guid menuCategoryId, string name, string description, int price)
        {
            return new Cuisine
            {
                CuisineId = cuisineId,
                Name = name,
                MenuCategoryId = menuCategoryId,
                Description = description,
                Price  = price,
                CreatedBy = string.Empty,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = string.Empty,
                UpdatedDate = DateTime.UtcNow
            };
        }
    }
}
