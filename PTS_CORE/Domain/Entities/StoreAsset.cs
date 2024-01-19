using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class StoreAsset : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StoreItemId { get; set; }
        public string StoreItemName { get; set; }
        public string? StoreItemDescription { get; set; }
        public string TerminalId { get; set; }
        public string TerminalName { get; set; }
        public long TotalQuantity { get; set; } 
        public long LastQuantityAdded { get; set; }
    }
}
