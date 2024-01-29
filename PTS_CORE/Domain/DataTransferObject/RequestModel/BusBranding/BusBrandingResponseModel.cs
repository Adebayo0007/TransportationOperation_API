using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.BusBranding
{
    public class BusBrandingResponseModel
    {
        public string Id { get; set; }  
        public string PartnerName { get; set; }
        public long NumberOfVehicle { get; set; }
        public DateTime BrandStartDate { get; set; }
        public DateTime BrandEndDate { get; set; }
        public bool IsActive { get; set; }
        public OperationType OperationType { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Reciept { get; set; }
        public double Amount { get; set; }
        public bool IsApprove { get; set; }

        public bool IsDeleted { get; set; } 
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorId { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsModified { get; set; }
        public string? ModifierName { get; set; }
        public string? ModifierId { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
