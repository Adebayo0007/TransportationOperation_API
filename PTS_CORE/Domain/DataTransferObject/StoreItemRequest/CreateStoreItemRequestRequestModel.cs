using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.StoreItemRequest
{
    public class CreateStoreItemRequestRequestModel
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string TerminalName { get; set; }
        [Required]
        public string StoreItemId { get; set; }
        [Required]
        public string ReasonForRequest { get; set; }
        [Required]
        public RequestType? RequestType { get; set; }
        [Required]
        public long Quantity { get; set; }
        [Required]
        public StoreItemType? StoreItemType { get; set; }
        public string? VehicleRegistrationNumber { get; set; }
    }
}
