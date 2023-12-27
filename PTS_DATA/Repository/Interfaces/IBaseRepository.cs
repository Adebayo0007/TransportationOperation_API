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
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
       // Task<T> GetByAnyAsync(Func<T,bool> any);
    }
}
