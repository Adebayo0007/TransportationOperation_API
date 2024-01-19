using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class StaffAssets : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StoreItemName { get; set; }
        public string Owner { get; set; }
        public int Quantity { get; set; }
    }
}
