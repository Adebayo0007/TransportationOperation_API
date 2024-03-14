using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class Fuel : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; set; }
        public string VehicleRegNumber { get; set; }
    }
}
