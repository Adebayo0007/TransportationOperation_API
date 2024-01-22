using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.StoreItemRequest;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IStoreItemRequestService
    {
        Task<bool> Create(CreateStoreItemRequestRequestModel model);
        Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> GetAllStoreItemRequests(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> GetInactiveStoreItemRequest(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> ResolvedStoreItemRequest(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> SearchStoreItemRequests(string? keyword, CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StoreItemRequestResponseModel>>> MystoreItemRequest(CancellationToken cancellationToken = default);
        Task<bool> UpdateStoreItemRequest(UpdateStoreItemRequestRequestModel updateModel);
        Task<bool> ApproveRequest(string id);
        Task<bool> ActivateStoreItemRequest(string id);
        Task<bool> Delete(string id);
    }
}
