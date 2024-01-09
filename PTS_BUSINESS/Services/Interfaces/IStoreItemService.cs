using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IStoreItemService
    {
        Task<bool> Create(CreateStoreItemRequestModel model);
        Task<BaseResponse<IEnumerable<StoreItemResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<StoreItemResponseModel>>> GetAllStoreItems(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StoreItemResponseModel>>> GetInactiveStoreItem(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<StoreItemResponseModel>>> SearchStoreItems(string? keyword,CancellationToken cancellationToken = default);
        Task<bool> UpdateStoreItem(UpdateStoreItemRequestModel updateModel);
        Task<bool> ActivateStoreItem(string id);
        Task<bool> Delete(string id);
    }
}
