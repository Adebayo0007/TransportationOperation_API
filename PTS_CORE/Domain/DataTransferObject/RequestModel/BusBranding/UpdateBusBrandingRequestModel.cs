using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.BusBranding
{
    public class UpdateBusBrandingRequestModel
    {
        [Required]
        public string Id { get; set; }
        public long? NumberOfVehicle { get; set; }
        public DateTime? BrandStartDate { get; set; }
        public DateTime? BrandEndDate { get; set; }
        public OperationType? OperationType { get; set; }
        public VehicleType? VehicleType { get; set; }
        public string? Reciept { get; set; }
        public double? Amount { get; set; }
    }
}
