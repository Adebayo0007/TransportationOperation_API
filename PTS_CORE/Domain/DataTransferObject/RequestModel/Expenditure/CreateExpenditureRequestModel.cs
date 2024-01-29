using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Expenditure
{
    public class CreateExpenditureRequestModel
    {
        [Required]
        public string Purpose { get; set; }
        public decimal? UnitPrice { get; set; }
        public long? ItemQuantity { get; set; }
        public string? StoreItemId { get; set; }
        public string? StoreItemName { get; set; }
        public string? TerminalId { get; set; }
        public string? TerminalName { get; set; }
    }
}
