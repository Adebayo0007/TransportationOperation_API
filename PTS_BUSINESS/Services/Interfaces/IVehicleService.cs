using PTS_CORE.Domain.DataTransferObject.RequestModel.Employee;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Vehicle;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<bool> Create(CreateVehicleRequestModel model);
        Task<BaseResponse<IEnumerable<VehicleResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<VehicleResponseModel>>> GetAllVehicles(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<VehicleResponseModel>>> GetInactiveVehicle(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<VehicleResponseModel>>> GetTerminalVehicle(string terminalId,CancellationToken cancellationToken = default);
        Task<bool> UpdateVehicle(UpdateVehicleRequestModel updateModel);
        Task<bool> ActivateVehicle(string vehicleId);
        Task<bool> Delete(string id);
    }
}
