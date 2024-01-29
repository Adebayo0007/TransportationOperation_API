using Microsoft.EntityFrameworkCore;
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
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.BudgetTrackings.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;

        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<BudgetTracking> FindBudget(DateTime? resolvedDate)
        {
            return await _db.BudgetTrackings
            .SingleOrDefaultAsync(x => x.StartDate >= resolvedDate.Value && resolvedDate.Value <= x.EndDate);
        }


        public async Task<IEnumerable<BudgetTracking>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.BudgetTrackings
           .Where(x => x.StartDate.Date.Year == DateTime.Now.Date.Year)
           .OrderByDescending(x => x.DateCreated)
           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BudgetTracking>> GetByIdAsync(string id)
        {
            var result = await _db.BudgetTrackings
           .Where(x => x.Id.ToLower() == id.ToLower())
           .ToListAsync();
            return result ?? null;
        }

        public async Task<BudgetTracking> GetModelByIdAsync(string id)
        {
            return await _db.BudgetTrackings
             .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<BudgetTracking>> InactiveBudgetTrackings(CancellationToken cancellationToken = default)
        {
            return await _db.BudgetTrackings
            .Where(x => x.IsDeleted == true)
            .OrderByDescending(x => x.DeletedDate)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BudgetTracking>> SearchBudgetTrackings(DateTime? start, DateTime? to, CancellationToken cancellationToken = default)
        {
            return await _db.BudgetTrackings
            .Where(x => x.StartDate >= start && x.EndDate <= to)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(BudgetTracking entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
