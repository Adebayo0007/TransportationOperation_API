using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.HireVehicle
{
    public class CreateHireVehicleRequestModel
    {
        [Required]
        public string Customer { get; set; }
        [Required]
        public string DepartureAddress { get; set; }
        [Required]
        public string DestinationAddress { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]  
        public double Profit { get; set; }
        [Required]  
        public double CostOfExacution { get; set; }
        [Required]
        public double Fuel { get; set; }
        [Required]
        public string RecieptAndInvoice { get; set; }
        [Required]
        public DateTime DeapartureDate { get; set; }
        [Required]
        public DateTime ArrivalDate { get; set; }
        [Required]
        public string DepartureTerminalName { get; set; }
        [Required]
        public OperationType OperationType { get; set; }
    }
}
