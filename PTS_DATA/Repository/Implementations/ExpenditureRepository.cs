using Microsoft.EntityFrameworkCore;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PTS_DATA.Repository.Implementations
{
    public class ExpenditureRepository : IExpenditureRepository
    {
        private readonly ApplicationDBContext _db;
        public ExpenditureRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Expenditure entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Expenditures.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Expenditure>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Expenditures
             .Where(x => x.IsDeleted == false)
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Expenditure>> GetByIdAsync(string id)
        {
            return await _db.Expenditures
                            .Where(x => x.Id == id)
                            .ToListAsync();
        }

        public async Task<Expenditure> GetModelByIdAsync(string id)
        {
            return await _db.Expenditures
                       .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task UpdateAsync(Expenditure entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
