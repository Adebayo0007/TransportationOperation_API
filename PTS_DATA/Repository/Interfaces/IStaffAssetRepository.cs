using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IStaffAssetRepository : IBaseRepository<StaffAssets>
    {
        Task<StaffAssets> GetModelByIdAsync(string id);
        Task<IEnumerable<StaffAssets>> InactiveStaffAsset(CancellationToken cancellationToken = default);
        Task<IEnumerable<StaffAssets>> SearchStaffAsset(string? keyword, CancellationToken cancellationToken = default);
    }
}
