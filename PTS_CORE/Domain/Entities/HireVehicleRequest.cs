using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class HireVehicleRequest :RequestSetting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 7);
        public HireVehicle HireVehicle { get; set; }
        public double Profit { get; set; }
        public double Cost { get; set; }
        public double Maintenance { get; set; }
        public double Fuel { get; set; }
        public string RecieptImage { get; set; }
    }
}
