using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDTOs
{
    public class WebsiteDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string CustomerKey { get; set; }
        public string CustomerSecret { get; set; }
        public bool HasApiAccess { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
