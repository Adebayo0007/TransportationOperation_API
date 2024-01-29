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
    public class BusBrandingRequestModel
    {
        [Required]
        public string PartnerName { get; set; }
        [Required]
        public long NumberOfVehicle { get; set; }
        [Required]
        public DateTime BrandStartDate { get; set; }
        [Required]
        public DateTime BrandEndDate { get; set; }
        [Required]  
        public OperationType OperationType { get; set; }
        [Required]  
        public VehicleType VehicleType { get; set; }
        [Required]
        public string Reciept { get; set; }
        [Required]  
        public double Amount { get; set; }
        public bool IsApprove { get; set; }
    }
}
