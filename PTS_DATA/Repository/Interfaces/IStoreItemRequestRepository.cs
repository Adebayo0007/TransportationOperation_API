using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IStoreItemRequestRepository : IBaseRepository<StoreItemRequest>
    {
        Task<StoreItemRequest> GetModelByIdAsync(string id);
        Task<IEnumerable<StoreItemRequest>> InactiveStoreItemRequest(CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreItemRequest>> SearchStoreItemRequest(string? keyword, CancellationToken cancellationToken = default);
    }
}
