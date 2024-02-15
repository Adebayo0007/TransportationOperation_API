using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
