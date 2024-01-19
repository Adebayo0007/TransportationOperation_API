using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class StoreAssetRequest : RequestSetting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public StoreAsset StoreAsset { get; set; }
    }
}
