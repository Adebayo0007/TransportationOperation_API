using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Expenditure
{
    public class ExpenditureResponseModel
    {

        public string Id { get; set; }
        public string Purpose { get; set; }
        public decimal? UnitPrice { get; set; }
        public long? ItemQuantity { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? StoreItemId { get; set; }
        public string? StoreItemName { get; set; }
        public string? TerminalId { get; set; }
        public string? TerminalName { get; set; }

        public bool? IsChairmanApproved { get; set; }
        public RequestType RequestType { get; set; }
        public bool? IsDDPCommented { get; set; } 
        public string? DDPComment { get; set; }
        public bool? IsAuditorCommented { get; set; }
        public string? AuditorComment { get; set; }
        public bool? IsProcurementApproved { get; set; } 
        public bool? IsResolved { get; set; } 
        public bool? IsDenied { get; set; } 
        public bool? IsApproved { get; set; }
        public bool? IsVerified { get; set; } 

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
