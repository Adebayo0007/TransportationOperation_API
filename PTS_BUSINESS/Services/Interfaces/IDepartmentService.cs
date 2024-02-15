using PTS_CORE.Domain.DataTransferObject.RequestModel.StoreItem;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Depratment;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<bool> Create(CreateDepartmentalSaleRequestModel model);
        Task<BaseResponse<IEnumerable<DepartmentResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<DepartmentResponseModel>>> GetAllDepartments(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<DepartmentResponseModel>>> GetInactiveDepartments(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<DepartmentResponseModel>>> SearchDepartment(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> UpdateDepartment(UpdateDepartmentRequestModel updateModel);
        Task<bool> ActivateDepartment(string id);
        Task<bool> Delete(string id);
    }
}
