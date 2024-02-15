using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalExpenditure;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.DepartmentalSale;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface IDepartmentalSaleService
    {
        Task<bool> Create(CreateDepartmentalSale model);
        Task<BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>> GetAllDepartmentalSales(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>> GetInactiveDepartmentalSales(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<DepartmentalSaleResponseModel>>> SearchDepartmentalSales(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> UpdateDepartmentalSale(UpdateDepartmentalSaleRequestModel updateModel);
        Task<bool> ActivateDepartmentalSale(string id);
        Task<bool> Delete(string id);
    }
}
