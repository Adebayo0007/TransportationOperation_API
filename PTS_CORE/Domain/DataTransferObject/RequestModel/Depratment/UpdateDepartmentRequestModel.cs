using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Depratment
{
    public class UpdateDepartmentRequestModel
    {
        [Required]  
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
