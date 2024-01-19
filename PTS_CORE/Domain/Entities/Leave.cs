using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class Leave: BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Subject { get; set; }
        public LeaveType LeaveType { get; set; }
        public string ReasonForLeave { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime Enddate { get; set; }
        public bool IsOpened { get; set; } = false;
        public bool IsGranted { get; set; } = false;
        public bool IsDenied { get; set; } = false;
        public string? ReasonForDenial { get; set; }
    }
}
