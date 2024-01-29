using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.BudgetTraching
{
    public class SearchBudgetRequestModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
