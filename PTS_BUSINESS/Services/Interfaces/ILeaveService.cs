using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface ILeaveService
    {
        Task<bool> Create(CreateLeaveRequestModel model);
        Task<BaseResponse<IEnumerable<LeaveResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<LeaveResponseModel>>> GetAllLeaves(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<LeaveResponseModel>>> GetInactiveLeaves(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<LeaveResponseModel>>> SearchLeaves(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> Update(UpdateLeaveRequestModel updateModel);
        Task<bool> ActivateLeave(string id);
        Task Delete(string id);
        Task IsOpened(string id);
        Task IsGranted(string id);
        Task IsDenied(string id);
    }
}
