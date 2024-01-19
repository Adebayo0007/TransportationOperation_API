using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.StaffAsset
{
    public class CreateStaffAssetRequestModel
    {
        [Required]
        public string StoreItemName { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
