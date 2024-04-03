using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Fuel
{
    public class CreateFuelRequestModel
    {
        [Required]
        public string VehicleRegNumber { get; set; }
        [Required]
        public double Quantity { get; set; }
    }
}
