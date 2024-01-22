using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IBudgetTrackingRepository : IBaseRepository<BudgetTracking>
    {
        Task<BudgetTracking> GetModelByIdAsync(string id);
        Task<IEnumerable<BudgetTracking>> InactiveBudgetTrackings(CancellationToken cancellationToken = default);
        Task<IEnumerable<BudgetTracking>> SearchBudgetTrackings(DateTime? start,DateTime? to, CancellationToken cancellationToken = default);
    }
}
