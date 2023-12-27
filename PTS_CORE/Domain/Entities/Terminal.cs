using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class Terminal : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 7);
        public string Name { get; set; }
        public string? Code { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPerson2 { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public DateTime TerminalStartingDate { get; set; }
        public string State { get; set; }
        public bool IsCommission { get; set; }
    }
}
