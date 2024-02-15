using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalExpenditure;
using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IDepartmentalExpenditureBudgetService
    {
        Task<bool> Create(CreateDepartmentalExpenditure model);
        Task<BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>> GetAllDepartmentalExpenditure(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>> GetInactiveDepartmentalExpenditure(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<DepartmentalExpenditureResponseModel>>> SearchDepartmentalExpenditures(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> UpdateDepartmentalExpenditure(UpdateDepartmentalExpenditureRequestModel updateModel);
        Task<bool> ActivateDepartmentalExpenditure(string id);
        Task<bool> Delete(string id);
    }
}
