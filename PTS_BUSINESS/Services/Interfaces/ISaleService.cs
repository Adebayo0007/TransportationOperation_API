using PTS_CORE.Domain.DataTransferObject.RequestModel.Leave;
using PTS_CORE.Domain.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Sale;

namespace PTS_BUSINESS.Services.Interfaces
{
    public interface ISaleService
    {
        Task<bool> Create(CreateSaleRequestModel model);
        Task<BaseResponse<IEnumerable<SaleResponseModel>>> Get(string id);
        Task<BaseResponse<IEnumerable<SaleResponseModel>>> GetAllSales(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<SaleResponseModel>>> GetInactiveSales(CancellationToken cancellationToken = default);
        Task<BaseResponse<IEnumerable<SaleResponseModel>>> SearchSales(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> Update(UpdateSaleRequestModel updateModel);
        Task<bool> ActivateSale(string id);
        Task<bool> Delete(string id);
    }
}
