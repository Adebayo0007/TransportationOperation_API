using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.StoreItemRequest
{
    public class StoreItemRequestResponseModel
    {
        public string Id { get; set; } 
        public string Description { get; set; }
        public string TerminalName { get; set; }
        public string StoreItemId { get; set; }
        public string StoreItemName { get; set; }
        public string ReasonForRequest { get; set; }
        public long Quantity { get; set; }
        public StoreItemType StoreItemType { get; set; }
        public string? VehicleRegistrationNumber { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorId { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsModified { get; set; }
        public string? ModifierName { get; set; }
        public string? ModifierId { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsChairmanApproved { get; set; } 
        public RequestType RequestType { get; set; }
        public AvailabilityType? AvailabilityType { get; set; }
        public bool? IsDDPCommented { get; set; } 
        public string? DDPComment { get; set; }
        public bool? IsAuditorCommented { get; set; } 
        public string? AuditorComment { get; set; }
        public bool? IsResolved { get; set; } 
        public bool? IsAvailable { get; set; } 
        public bool? IsVerified { get; set; } 
    }
}
