using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public abstract class RequestSetting
    {
        public bool? IsChairmanApproved { get; set; }
        public RequestType RequestType { get; set; }
        public bool? IsDDPCommented { get; set; }
        public string? DDPComment { get; set; }
        public bool? IsAuditorCommented { get; set; }
        public string? AuditorComment { get; set; }
        public bool? IsResolved { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsVerified { get; set; }
    }
}
