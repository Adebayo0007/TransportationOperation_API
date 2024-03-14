using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IFuelRepository : IBaseRepository<Fuel>
    {
        Task<Fuel> GetModelByIdAsync(string id);
        Task<IEnumerable<Fuel>> InactiveFuels(CancellationToken cancellationToken = default);
        Task<IEnumerable<Fuel>> SearchFuels(DateTime? keyword, CancellationToken cancellationToken = default);
    }
}
