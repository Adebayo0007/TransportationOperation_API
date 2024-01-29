using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Expenditure;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IExpenditureService
    {
        Task<bool> Create(CreateExpenditureRequestModel model);
        Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> GetAllExpenditures(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> GetInactiveExpenditures(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> ResolvedRequest(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<ExpenditureResponseModel>>> SearchExpenditures(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> Update(UpdateExpenditureRequestModel updateModel);
        Task<bool> ActivateExpenditure(string id);
        Task<bool> Delete(string id);
        Task IsGranted(string id);
        Task IsVerified(string id);
        Task Resolve(string id);
        Task IsDenied(string id);
    }
}
