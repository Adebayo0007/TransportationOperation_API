using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class OtherRequest : RequestSetting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 7);
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
