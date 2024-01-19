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
    public class StoreItemRepository : IStoreItemRepository
    {
        private readonly ApplicationDBContext _db;
        public StoreItemRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(StoreItem entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.StoreItems.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<StoreItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.StoreItems
             .Where(x => x.IsDeleted == false)
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItem>> GetByIdAsync(string id)
        {
            var result = await _db.StoreItems
            .Where(x => x.Id.ToLower() == id.ToLower())
            .ToListAsync();
            return result ?? null;
        }

        public async Task<StoreItem> GetModelByIdAsync(string id)
        {
            return await _db.StoreItems
              .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<StoreItem>> InactiveStoreItem(CancellationToken cancellationToken = default)
        {
            return await _db.StoreItems
             .Where(x => x.IsDeleted == true)
             .OrderByDescending(x => x.DeletedDate)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItem>> SearchStoreItems(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.StoreItems
             .Where(x => x.Name.Contains(keyword.Trim()) || x.Name.ToLower() == keyword.Trim().ToLower())
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(StoreItem entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
