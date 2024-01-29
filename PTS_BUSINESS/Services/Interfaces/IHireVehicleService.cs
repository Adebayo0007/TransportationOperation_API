using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.HireVehicle;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IHireVehicleService
    {
        Task<bool> Create(CreateHireVehicleRequestModel model);
        Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> GetAllHireVehicles(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> GetInactiveSHireVehicles(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> UnApprovedHiring(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<HireVehicleResponseModel>>> SearchHireVehicles(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> UpdateHireVehicle(UpdateHireVehicleRequestModel updateModel);
        Task<bool> ActivateHireVehicle(string id);
        Task<bool> Approve(string id);
        Task<bool> ResolvedByDepo(string id);
        Task<bool> Delete(string id);
    }
}
