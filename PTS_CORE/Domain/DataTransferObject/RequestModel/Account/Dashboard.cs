using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_CORE.Domain.DataTransferObject.RequestModel.Account
{
    public class Dashboard
    {
        public decimal Sales { get; set; }
        public decimal TodaySales { get; set; }
        public decimal Expenditure { get; set; }
        public decimal TodayExpenditure { get; set; }
        public decimal Budget { get; set; }
        public long Vehicles { get; set; }
        public long BrandedBuses { get; set; }
        public long User { get; set; }
        public long Employee { get; set; }
        public long Terminals { get; set; }
        public long StoreItemRequestForAuditor { get; set; }
        public long StoreItemRequestForDDP { get; set; }
        public long StoreItemRequestForChairman { get; set; }
        public long StoreItemRequestForStore { get; set; }
        public long FinancialRequestForAuditor { get; set; }
        public long FinancialRequestForDDP { get; set; }
        public long FinancialRequestForChairman { get; set; }
        public long FinancialRequestForFinance { get; set; }
        public long FinancialRequestForProcumentOfficer { get; set; }
        public long MyStoreItem { get; set; }
        public long MyExpenditure { get; set; }
    }
}
