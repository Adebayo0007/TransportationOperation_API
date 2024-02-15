using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalExpenditure
{
    public class UpdateDepartmentalExpenditureRequestModel
    {
        [Required]
        public string Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? BudgetedAmount { get; set; }
    }
}
