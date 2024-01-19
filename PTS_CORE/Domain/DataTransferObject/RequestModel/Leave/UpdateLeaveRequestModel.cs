using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Leave
{
    public class UpdateLeaveRequestModel
    {

        [Required]
        public string Id { get; set; }
        public string? Subject { get; set; }
        public LeaveType? LeaveType { get; set; }
        public string? ReasonForLeave { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public string? ReasonForDenial { get; set; }
    }
}
