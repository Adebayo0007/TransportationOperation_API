using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class BusBrandingRequest : RequestSetting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public long NumberOfVehicle { get; set; }
        public DateTime BrandStartDate { get; set; }
        public DateTime BrandEndDate { get; set; }
        public bool IsActive { get; set; }
        public OperandType OperationType { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Reciept { get; set; }
        public double Amount { get; set; }
    }
}
