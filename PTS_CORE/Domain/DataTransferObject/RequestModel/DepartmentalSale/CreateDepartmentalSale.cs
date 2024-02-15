using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalSale
{
    public class CreateDepartmentalSale
    {
        [Required]
        public string DepartmentId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public decimal BudgetedAmount { get; set; }
    }
}
