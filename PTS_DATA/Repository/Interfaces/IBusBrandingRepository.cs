using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IBusBrandingRepository : IBaseRepository<BusBranding>
    {
        Task<BusBranding> GetModelByIdAsync(string id);
        Task<IEnumerable<BusBranding>> InactiveBranding(CancellationToken cancellationToken = default);
        Task<IEnumerable<BusBranding>> UnApprovedBranding(CancellationToken cancellationToken = default);
        Task<IEnumerable<BusBranding>> SearchBranding(string? keyword, CancellationToken cancellationToken = default);
    }
}
