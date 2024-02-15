using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public  interface IDepartmentalSaleRepository : IBaseRepository<DepartmentalSale>
    {
        Task<DepartmentalSale> GetModelByIdAsync(string id);
        Task<DepartmentalSale> GetModelByDepartmentIdAsync(string id);
        Task<IEnumerable<DepartmentalSale>> InactiveBudgetTrackings(CancellationToken cancellationToken = default);
        Task<decimal> ThisYearBudgetTrackings(string? id,CancellationToken cancellationToken = default);
        Task<IEnumerable<DepartmentalSale>> SearchBudgetTrackings(string? keyword, CancellationToken cancellationToken = default);
    }
}
