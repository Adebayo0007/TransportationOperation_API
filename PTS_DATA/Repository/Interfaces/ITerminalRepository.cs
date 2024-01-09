using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface ITerminalRepository : IBaseRepository<Terminal>
    {
        Task<Terminal> GetModelByIdAsync(string id);
        Task<IEnumerable<Terminal>> InactiveTerminal(CancellationToken cancellationToken = default);
        Task<IEnumerable<Terminal>> SearchTerminal(string? keyword= null, CancellationToken cancellationToken = default);
    }
}
