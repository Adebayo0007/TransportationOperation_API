using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IHireVehicleRepository : IBaseRepository<HireVehicle>
    {
        Task<HireVehicle> GetModelByIdAsync(string id);
        Task<IEnumerable<HireVehicle>> InactiveHiredVeehicle(CancellationToken cancellationToken = default);
        Task<IEnumerable<HireVehicle>> UnApprovedHiring(CancellationToken cancellationToken = default);
        Task<IEnumerable<HireVehicle>> GetAllForChairman(CancellationToken cancellationToken = default);
        Task<IEnumerable<HireVehicle>> GetAllForOperation(CancellationToken cancellationToken = default);
        Task<IEnumerable<HireVehicle>> GetAllForDepo(CancellationToken cancellationToken = default);
        Task<IEnumerable<HireVehicle>> SearchHiredVehicles(string? keyword, CancellationToken cancellationToken = default);
    }
}
