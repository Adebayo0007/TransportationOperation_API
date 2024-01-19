using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Vehicle
{
    public class UpdateVehicleRequestModel
    {
        [Required]
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? DriverId { get; set; }
        public string? TerminalId { get; set; }
        public OperationType? OperationType { get; set; }
        public VehicleStatus? VehicleStatus { get; set; }
        public VehicleType? VehicleType { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? EngineNumber { get; set; }
        public string? IMEINumber { get; set; }
        public string? VehicleModel { get; set; }
        public int? NumberOfSeat { get; set; }
        public DateTime? LicenseDate { get; set; }
        public DateTime? LicenseExpirationDate { get; set; }
        public DateTime? InsuranceDate { get; set; }
        public DateTime? RoadWorthinessDate { get; set; }
        public DateTime? RoadWorthinessExpirationDate { get; set; }
    }
}
