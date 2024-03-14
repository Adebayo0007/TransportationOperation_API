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
    public class FuelRepository : IFuelRepository
    {
        private readonly ApplicationDBContext _db;
        public FuelRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Fuel entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Fuels.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Fuel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Fuels
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Fuel>> GetByIdAsync(string id)
        {
            var result = await _db.Fuels
          .Where(x => x.Id.ToLower() == id.ToLower())
          .ToListAsync();
            return result ?? null;
        }

        public async Task<Fuel> GetModelByIdAsync(string id)
        {
            return await _db.Fuels
              .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<Fuel>> InactiveFuels(CancellationToken cancellationToken = default)
        {
            return await _db.Fuels
             .Where(x => x.IsDeleted == true)
             .OrderByDescending(x => x.DeletedDate)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Fuel>> SearchFuels(DateTime? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.Fuels
             .Where(x => x.DateCreated.Value.Date == keyword.Value.Date)
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Fuel entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
