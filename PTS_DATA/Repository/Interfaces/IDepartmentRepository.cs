using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        Task<Department> GetModelByIdAsync(string id);
        Task<IEnumerable<Department>> InactiveDepartments(CancellationToken cancellationToken = default);
        Task<IEnumerable<Department>> SearchDepartments(string? keyword, CancellationToken cancellationToken = default);
    }
}
