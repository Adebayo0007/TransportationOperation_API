using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IOtherRequestRepository : IBaseRepository<OtherRequest>
    {
        Task<OtherRequest> GetModelByIdAsync(string id);
        Task<IEnumerable<OtherRequest>> SearchOtherRequests(string? keyword, CancellationToken cancellationToken = default);
        Task<IEnumerable<OtherRequest>> MyRequest(string? keyword, CancellationToken cancellationToken = default);
        Task<IEnumerable<OtherRequest>> InactivatedOtherRequest(CancellationToken cancellationToken = default);
    }
}
