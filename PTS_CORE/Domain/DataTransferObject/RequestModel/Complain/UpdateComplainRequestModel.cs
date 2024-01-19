using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Complain
{
    public class UpdateComplainRequestModel
    {
        [Required]
        public string Id { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
    }
}
