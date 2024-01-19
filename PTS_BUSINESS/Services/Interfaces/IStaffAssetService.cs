using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.StaffAsset;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IStaffAssetService
    {
        Task<bool> Create(CreateStaffAssetRequestModel model);
        Task<BaseResponse<IEnumerable<StaffAssetResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<StaffAssetResponseModel>>> GetAllStaffAsset(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StaffAssetResponseModel>>> GetInactiveStaffAsset(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StaffAssetResponseModel>>> SearchStaffAsset(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> ActivateStaffAsset(string id);
        Task<bool> Delete(string id);
    }
}
