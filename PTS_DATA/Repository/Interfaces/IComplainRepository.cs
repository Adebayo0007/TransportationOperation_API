using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IComplainRepository : IBaseRepository<Complain>
    {
        Task<Complain> GetModelByIdAsync(string id);
        Task<IEnumerable<Complain>> InactiveComplains(CancellationToken cancellationToken = default);
        Task<IEnumerable<Complain>> SearchComplains(string? keyword, CancellationToken cancellationToken = default);
    }
}
