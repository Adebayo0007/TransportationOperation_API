using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Content { get; set; }
        public bool? RequiredChirman { get; set; }
        public bool? IsChirmanViewed{ get; set; }
        public bool? RequiredAdmin { get; set; }
        public bool? IsAdminViewed { get; set; }
        public bool? RequiredFinance { get; set; }
        public bool? IsFinanceViewed { get; set; }
    }
}
