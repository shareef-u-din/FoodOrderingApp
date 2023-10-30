using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Responses
{
    public class MenuCategoryResponse
    {
        public Guid MenuId { get; set; }
        public Guid MenuCategoryId { get; set; }
        public string Title { get; set; }
        public List<CuisineResponse> Cuisines { get; set; }
    }
}
