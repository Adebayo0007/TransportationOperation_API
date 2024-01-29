using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ApplicatioUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string? TerminalId { get; set; }
        public string? StaffIdentityCardNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
