using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.BudgetTraching;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IBudgetTrackingService
    {
        Task<bool> Create(CreateBudgetTrackingRequestModel model);
        Task<BaseResponse<IEnumerable<BudgetTrackingResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<BudgetTrackingResponseModel>>> GetAllBudgetTrackings(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<BudgetTrackingResponseModel>>> GetInactiveBudgetTrackings(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<BudgetTrackingResponseModel>>> SearchBudgetTrackings(DateTime start,DateTime to,CancellationToken cancellationToken = default);
        Task<bool> UpdateBudgetTracking(UpdateBudgetTrackingRequestModel updateModel);
        Task<bool> ActivateBudgetTracking(string id);
        Task<bool> Delete(string id);
    }
}   
