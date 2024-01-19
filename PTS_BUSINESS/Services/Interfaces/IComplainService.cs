using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Complain;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IComplainService
    {
        Task<bool> Create(CreateComplainRequestModel model);
        Task<BaseResponse<IEnumerable<ComplainResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<ComplainResponseModel>>> GetAllComplains(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<ComplainResponseModel>>> GetInactiveComplains(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<ComplainResponseModel>>> SearchComplains(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> Update(UpdateComplainRequestModel updateModel);
        Task<bool> ActivateComplains(string id);
        Task Delete(string id);
    }
}
