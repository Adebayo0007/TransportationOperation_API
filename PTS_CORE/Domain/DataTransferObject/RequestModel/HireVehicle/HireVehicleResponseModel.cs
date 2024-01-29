using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.HireVehicle
{
    public class HireVehicleResponseModel
    {
        public string Id { get; set; } 
        public string Customer { get; set; }
        public string DepartureAddress { get; set; }
        public string DestinationAddress { get; set; }
        public double Amount { get; set; }
        public string? VehicleId { get; set; }
        public double Profit { get; set; }
        public double CostOfExacution { get; set; }
        public double Fuel { get; set; }
        public string RecieptAndInvoice { get; set; }
        public string? DriverUserId { get; set; }
        public DateTime DeapartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string DepartureTerminalName { get; set; }
        public bool IsChairmanApprove { get; set; } 
        public bool ResolvedByOperation { get; set; }
        public bool ResolvedByDepo { get; set; }
        public OperationType OperationType { get; set; }

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
