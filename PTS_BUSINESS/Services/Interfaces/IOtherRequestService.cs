using PTS_CORE.Domain.DataTransferObject.RequestModel.Complain;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.OtherRequest;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IOtherRequestService
    {
        Task<bool> Create(CreateOtherRequestRequestModel model);
        Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> GetAllOtherRequests(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> GetInactiveOtherRequests(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> SearchOtherRequests(string? keyword, CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<OtherRequestResponseModel>>> MyRequest(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> Update(UpdateOtheRequestRequestModel updateModel);
        Task<bool> ActivateOtherRequests(string id);
        Task<bool> ChairmanApproving(string id);
        Task Delete(string id);
    }
}
