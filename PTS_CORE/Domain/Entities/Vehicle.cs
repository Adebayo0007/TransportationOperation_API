using PTS_CORE.Domain.Entities.Enum;

namespace PTS_CORE.Domain.Entities
{
    public class Vehicle : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }  
        public string? DriverId { get; set; }
        public string? TerminalId { get; set; }
        public OperationType? OperationType { get; set; }
        public VehicleStatus? VehicleStatus { get; set; }
        public VehicleType? VehicleType { get; set; }
        public string RegistrationNumber { get; set; }
        public string EngineNumber { get; set; }
        public string? IMEINumber { get; set; }
        public string VehicleModel { get; set; }
        public int NumberOfSeat { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? LicenseDate { get; set; }
        public DateTime? LicenseExpirationDate { get; set; }
        public DateTime? InsuranceDate { get; set; }
        public DateTime? RoadWorthinessDate { get; set; }
        public DateTime? RoadWorthinessExpirationDate { get; set; }
    }
}
