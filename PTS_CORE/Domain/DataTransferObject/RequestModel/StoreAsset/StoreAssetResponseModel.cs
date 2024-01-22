using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.StoreAsset
{
    public class StoreAssetResponseModel
    {
        public string Id { get; set; } 
        public string StoreItemId { get; set; }
        public string StoreItemName { get; set; }
        public string? StoreItemDescription { get; set; }
        public string TerminalId { get; set; }
        public string TerminalName { get; set; }
        public long TotalQuantity { get; set; }
        public long LastQuantityAdded { get; set; }

        public bool IsDeleted { get; set; } = false;
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
