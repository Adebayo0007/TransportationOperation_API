using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.StoreAsset
{
    public class CreateStoreAssetRequestModel
    {
        [Required]
        public string StoreItemId { get; set; }
        [Required]
        public string TerminalId { get; set; }
        [Required]
        public long TotalQuantity { get; set; }
    }
}
