using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.StoreItemRequest
{
    public class UpdateStoreItemRequestRequestModel
    {
        [Required]
        public string Id { get; set; }
        public string? Description { get; set; }
     /*   public string? TerminalName { get; set; }
        public string? StoreItemId { get; set; }*/
        public string? ReasonForRequest { get; set; }
        public long? Quantity { get; set; } 
       // public StoreItemType? StoreItemType { get; set; }
        //public string? VehicleRegistrationNumber { get; set; }
        public AvailabilityType? AvailabilityType { get; set; }
        public string? DDPComment { get; set; }
        public string? AuditorComment { get; set; }
    }
}
