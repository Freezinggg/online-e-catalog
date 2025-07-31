using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Domain.Entities
{
    public class CatalogItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public bool InStock { get; set; } = false;
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateTime { get; set; }
    }
}
