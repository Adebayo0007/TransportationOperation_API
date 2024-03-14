using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Fuel
{
    public class UpdateFuelRequestModel
    {
        [Required]
        public string Id { get; set; }
        public string? VehicleRegNumber { get; set; }
        public int? Quantity { get; set; }
    }
}
