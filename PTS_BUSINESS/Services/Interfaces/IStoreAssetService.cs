using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;
using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreAsset;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IStoreAssetService
    {
        Task<bool> Create(CreateStoreAssetRequestModel model);
        Task<BaseResponse<IEnumerable<StoreAssetResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<StoreAssetResponseModel>>> GetAllStoreAsset(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StoreAssetResponseModel>>> GetInactiveStoreAsset(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StoreAssetResponseModel>>> SearchStoreAsset(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> ActivateStoreAsset(string id);
        Task<bool> Delete(string id);
    }
}
