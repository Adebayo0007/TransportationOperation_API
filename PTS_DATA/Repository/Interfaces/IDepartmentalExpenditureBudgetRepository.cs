using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IDepartmentalExpenditureBudgetRepository : IBaseRepository<DepartmentalExpenditureBudget>
    {
        Task<DepartmentalExpenditureBudget> GetModelByIdAsync(string id);
        Task<DepartmentalExpenditureBudget> GetModelByDepartmentIdAsync(string id);
        Task<IEnumerable<DepartmentalExpenditureBudget>> InactiveBudgetTrackings(CancellationToken cancellationToken = default);
        Task<decimal> ThisYearBudgetTrackings(string? id,CancellationToken cancellationToken = default);
        Task<IEnumerable<DepartmentalExpenditureBudget>> SearchBudgetTrackings(string? keyword, CancellationToken cancellationToken = default);
    }
}
