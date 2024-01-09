using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee> GetModelByIdAsync(string id);
        Task<IEnumerable<Employee>> GetByEmailAsync(string email);
        Task<IEnumerable<Employee>> InactiveEmployees(CancellationToken cancellationToken = default);
        Task<IEnumerable<Employee>> SearchEmployees(string? keyword, CancellationToken cancellationToken = default);
    }
}
