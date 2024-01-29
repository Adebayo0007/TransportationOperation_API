using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Employee
{
    public class EmployeeResponseModel
    {
        public string Id { get; set; }
        public string StaffIdentityCardNumber { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public string Phonenumber { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
        public string? ApplicationRoleId { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorId { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsModified { get; set; }
        public string? ModifierName { get; set; }
        public string? ModifierId { get; set; }
        public string? TerminalId { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}
