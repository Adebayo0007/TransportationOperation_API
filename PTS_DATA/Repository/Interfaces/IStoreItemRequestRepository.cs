using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IStoreItemRequestRepository : IBaseRepository<StoreItemRequest>
    {
        Task<StoreItemRequest> GetModelByIdAsync(string id);
        Task<IEnumerable<StoreItemRequest>> InactiveStoreItemRequest(CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreItemRequest>> GetAllForAuditor(CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreItemRequest>> GetAllForDDP(CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreItemRequest>> GetAllForChirman(CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreItemRequest>> GetAllForStore(CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreItemRequest>> MystoreItemRequest(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<StoreItemRequest>> SearchStoreItemRequest(string? keyword, CancellationToken cancellationToken = default);
        Task<long> NumberOfRequestForStore();
        Task<long> NumberOfRequestForAuditor();
        Task<long> NumberOfRequestForDDP();
        Task<long> NumberOfRequestForChairman();
        Task<long> NumberOfMyrequest(string mail);
       
    }
}
