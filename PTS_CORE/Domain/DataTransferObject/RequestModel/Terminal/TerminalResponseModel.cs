using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal
{
    public class TerminalResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPerson2 { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public DateTime TerminalStartingDate { get; set; }
        public string State { get; set; }
        public bool IsCommission { get; set; }
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
    }
}
