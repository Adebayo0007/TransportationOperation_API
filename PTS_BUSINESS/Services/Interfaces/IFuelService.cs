using PTS_CORE.Domain.DataTransferObject.RequestModel.Fuel;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IFuelService
    {
        Task<bool> Create(CreateFuelRequestModel model);
        Task<BaseResponse<IEnumerable<FuelResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<FuelResponseModel>>> GetAllFuels(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<FuelResponseModel>>> GetInactiveFuels(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<FuelResponseModel>>> SearchFuels(DateTime? keyword, CancellationToken cancellationToken = default);
        Task<bool> UpdateFuel(UpdateFuelRequestModel updateModel);
        Task<bool> ActivateFuel(string id);
        Task<bool> Delete(string id);
    }
}
