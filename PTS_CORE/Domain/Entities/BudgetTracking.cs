using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class BudgetTracking : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BudgetedAmount { get; set; }
        public decimal ActualAmount { get; set; }
       // public decimal Difference { get; set; }
    }
}
