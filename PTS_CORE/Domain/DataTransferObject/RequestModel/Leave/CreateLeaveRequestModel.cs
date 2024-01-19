using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Leave
{
    public class CreateLeaveRequestModel
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public LeaveType LeaveType { get; set; }
        [Required]
        public string ReasonForLeave { get; set; }
        [Required]
        public DateTime Startdate { get; set; }
        [Required]
        public DateTime Enddate { get; set; }
    }
}
