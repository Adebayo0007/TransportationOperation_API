using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Implementations
{
    public class BudgetTrackingRepository : IBudgetTrackingRepository
    {
        private readonly ApplicationDBContext _db;
        public BudgetTrackingRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(BudgetTracking entity)
        {
           /* if (entity == null) throw new ArgumentNullException();
            var response = await _db.Bu.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;*/
            throw new NotImplementedException();
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BudgetTracking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BudgetTracking>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BudgetTracking> GetModelByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BudgetTracking>> InactiveBudgetTrackings(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BudgetTracking>> SearchBudgetTrackings(DateTime? start, DateTime? to, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(BudgetTracking entity)
        {
            throw new NotImplementedException();
        }
    }
}
