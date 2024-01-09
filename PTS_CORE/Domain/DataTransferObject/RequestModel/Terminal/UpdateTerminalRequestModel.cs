using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal
{
    public class UpdateTerminalRequestModel
    {
        [Required]
        public string Id { get; set; }  
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactPerson2 { get; set; }
        public float? Longitude { get; set; }
        public float? Latitude { get; set; }
        public DateTime? TerminalStartingDate { get; set; } = null;
    }
}
