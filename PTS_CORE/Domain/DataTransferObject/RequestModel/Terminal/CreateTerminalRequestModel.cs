using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Terminal
{
    public class CreateTerminalRequestModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string ContactPerson { get; set; }
        [Required]
        public string ContactPerson2 { get; set; }
        [Required]  
        public float Longitude { get; set; }
        [Required]
        public float Latitude { get; set; }
        [Required]  
        public DateTime TerminalStartingDate { get; set; }
        [Required]
        public string State { get; set; }
    
    }
}
