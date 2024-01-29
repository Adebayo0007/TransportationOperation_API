using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface ISaleRepository : IBaseRepository<Sales>
    {
        Task<Sales> GetModelByIdAsync(string id);
        Task<IEnumerable<Sales>> InactiveSaleRequest(CancellationToken cancellationToken = default);
        Task<IEnumerable<Sales>> SearchSale(string? keyword,CancellationToken cancellationToken = default);
    }
}
