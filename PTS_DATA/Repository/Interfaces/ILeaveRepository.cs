using PTS_CORE.Domain.DataTransferObject;
using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface ILeaveRepository :IBaseRepository<Leave>
    {
        Task<Leave> GetModelByIdAsync(string id);
        Task<IEnumerable<Leave>> InactiveLeaves(CancellationToken cancellationToken = default);
        Task<IEnumerable<Leave>> SearchLeaves(string? keyword, CancellationToken cancellationToken = default);
    }
}
