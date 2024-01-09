using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> CreateAsync(T entity);
        Task<IEnumerable<T>> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity);
        Task DeleteAsync();
       // Task<T> GetByAnyAsync(Func<T,bool> any);
    }
}
