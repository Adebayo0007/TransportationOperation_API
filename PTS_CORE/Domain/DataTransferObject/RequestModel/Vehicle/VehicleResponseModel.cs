using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Vehicle
{
    public class VehicleResponseModel
    {
            public string Id { get; set; }
            public string Name { get; set; }
            public string? DriverId { get; set; }
            public string? TerminalId { get; set; }
            public OperationType OperationType { get; set; }
            public VehicleStatus VehicleStatus { get; set; }
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
            public bool IsDeleted { get; set; }
            public DateTime? DeletedDate { get; set; }
            public string? DeletedBy { get; set; }
            public string? CreatorName { get; set; }
            public string? CreatorId { get; set; }
            public DateTime? DateCreated { get; set; }
            public bool? IsModified { get; set; }
            public string? ModifierName { get; set; }
            public string? ModifierId { get; set; }
            public DateTime? LastModified { get; set; }
    }
}
