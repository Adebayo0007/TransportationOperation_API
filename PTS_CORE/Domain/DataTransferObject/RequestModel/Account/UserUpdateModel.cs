using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Account
{
    public class UserUpdateModel
    {
        [Required]
        public string UserId { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RoleName { get; set; }
        public string? Email { get; set; }
        public string? ModifierName { get; set; }
        public string? ModifierId { get; set; }
        public string? Phonenumber { get; set; }
    }
}
