using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IStoreAssetRepository : IBaseRepository<StoreAsset>
    {
        Task<StoreAsset> GetModelByIdAsync(string id);
        Task<StoreAsset> GetStoreAssetByTerminalIdAndStoreItemid(string terminalId,string storeItemId);
        Task<IEnumerable<StoreAsset>> InactiveStoreAsset(CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreAsset>> SearchStoreAsset(string? keyword, CancellationToken cancellationToken = default);
    }
}
