using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IStoreItemRepository : IBaseRepository<StoreItem>
    {
        Task<StoreItem> GetModelByIdAsync(string id);
        Task<IEnumerable<StoreItem>> InactiveStoreItem(CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreItem>> SearchStoreItems(string? keyword, CancellationToken cancellationToken = default);
    }
}
