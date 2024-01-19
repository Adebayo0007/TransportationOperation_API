using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        Task<Vehicle> GetModelByIdAsync(string id);
        Task<IEnumerable<Vehicle>> GetTerminalVehicles(string terminalId,CancellationToken cancellationToken = default);
        Task<IEnumerable<Vehicle>> InactiveVehicle(CancellationToken cancellationToken = default);
        Task<IEnumerable<Vehicle>> SearchVehicle(string keyword, CancellationToken cancellationToken = default);
    }
}
