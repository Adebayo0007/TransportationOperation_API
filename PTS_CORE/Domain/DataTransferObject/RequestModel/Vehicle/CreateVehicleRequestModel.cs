using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Vehicle
{
    public class CreateVehicleRequestModel
    {
        [Required]
        public string Name { get; set; }
        public string? DriverId { get; set; }
        public string? TerminalId { get; set; }
        [Required]
        public OperationType OperationType { get; set; }
        [Required]
        public VehicleStatus VehicleStatus { get; set; }
        [Required]
        public string RegistrationNumber { get; set; }
        [Required]
        public string EngineNumber { get; set; }
        public string? IMEINumber { get; set; }
        [Required]
        public string VehicleModel { get; set; }
        [Required]
        public int NumberOfSeat { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        public DateTime? LicenseDate { get; set; }
        public DateTime? LicenseExpirationDate { get; set; }
        public DateTime? InsuranceDate { get; set; }
        public DateTime? RoadWorthinessDate { get; set; }
        public DateTime? RoadWorthinessExpirationDate { get; set; }
    }
}
