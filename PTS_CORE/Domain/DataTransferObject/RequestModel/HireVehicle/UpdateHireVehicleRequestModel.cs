using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.HireVehicle
{
    public class UpdateHireVehicleRequestModel
    {
        [Required]
        public string Id { get; set; }
        public string? DepartureAddress { get; set; }
        public string? DestinationAddress { get; set; }
        public double? Amount { get; set; }
        public string? VehicleId { get; set; }
        public double? Profit { get; set; }
        public double? CostOfExacution { get; set; }
        public double? Fuel { get; set; }
        public string? DriverUserId { get; set; }
        public DateTime? DeapartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public OperationType? OperationType { get; set; }
    }
}
