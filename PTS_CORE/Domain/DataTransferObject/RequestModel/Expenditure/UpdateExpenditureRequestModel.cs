using PTS_CORE.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Expenditure
{
    public class UpdateExpenditureRequestModel
    {
        [Required]
        public string Id { get; set; }
        public string? Purpose { get; set; }
        public decimal? UnitPrice { get; set; }
        public long? ItemQuantity { get; set; }
        public string? DDPComment { get; set; }
        public string? AuditorComment { get; set; }
    }
}
