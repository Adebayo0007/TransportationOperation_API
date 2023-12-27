using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class StoreItemRequest : RequestSetting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 7);
        public string TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string StoreItemId { get; set; }
        public string StoreItemName { get; set; }
        public string ReasonForRequest { get; set; }
        public string StoreAssetSignature { get; set; }
        public long Quantity { get; set; }
        public bool? IsTechnical { get; set; } 
        public string VehicleId { get; set; } 
    }
}
