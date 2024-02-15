using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.Entities
{
    public class DepartmentalExpenditureBudget : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BudgetedAmount { get; set; }
        public decimal? ActualAmount { get; set; }
    }
}
