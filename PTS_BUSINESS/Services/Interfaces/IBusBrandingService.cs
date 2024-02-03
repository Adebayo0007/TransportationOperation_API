using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.BusBranding;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IBusBrandingService
    {
        Task<bool> Create(BusBrandingRequestModel model);
        Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> GetAllBusBrandings(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> GetInactiveBusBranding(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> SearchBusBranding(string? keyword, CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<BusBrandingResponseModel>>> UnApprovedBranding(CancellationToken cancellationToken = default);
        Task<bool> UpdateBusBranding(UpdateBusBrandingRequestModel updateModel);
        Task<bool> ActivateBusBranding(string id);
        Task<bool> Approve(string id);
        Task MarkExpiredBrandAsDeleted();
        Task<bool> Delete(string id);
    }
}
