using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Responses
{
    public class MenuResponse
    {
        public Guid MenuId { get; set; }
        public string Title { get; set; }
    }
}
