

using PTS_CORE.Domain.Entities.Enum;

namespace PTS_CORE.Domain.Entities
{
    public class Expenditure : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Purpose { get; set; }
        public decimal? UnitPrice { get; set; }
        public long? ItemQuantity { get; set; }
        public string? StoreItemId { get; set; }
        public string? StoreItemName { get; set; }
        public string? TerminalId { get; set; }
        public string? TerminalName { get; set; }

        public bool? IsChairmanApproved { get; set; } = false;
        public RequestType RequestType { get; set; }
        public bool? IsDDPCommented { get; set; } = false;
        public string? DDPComment { get; set; }
        public bool? IsAuditorCommented { get; set; } = false;
        public string? AuditorComment { get; set; }
        public bool? IsProcurementApproved { get; set; } = false;
        public bool? IsResolved { get; set; } = false;
        public bool? IsDenied { get; set; } 
        public bool? IsApproved { get; set; } = false;
        public bool? IsVerified { get; set; } = false;
    }
}
