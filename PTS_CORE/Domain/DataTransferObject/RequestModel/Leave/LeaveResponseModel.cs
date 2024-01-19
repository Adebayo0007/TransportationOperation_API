using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Leave
{
    public class LeaveResponseModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public LeaveType LeaveType { get; set; }
        public string ReasonForLeave { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public bool IsOpened { get; set; } = false;
        public bool IsGranted { get; set; } = false;
        public bool IsDenied { get; set; } = false;
        public string? ReasonForDenial { get; set; }
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
