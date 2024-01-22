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
    public class StaffAssetRepository : IStaffAssetRepository
    {
        private readonly ApplicationDBContext _db;
        public StaffAssetRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(StaffAssets entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.StaffAssets.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<StaffAssets>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.StaffAssets
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StaffAssets>> GetByIdAsync(string id)
        {
            var result = await _db.StaffAssets
            .Where(x => x.Id.ToLower() == id.ToLower())
            .ToListAsync();
            return result ?? null;
        }

        public async Task<StaffAssets> GetModelByIdAsync(string id)
        {
            return await _db.StaffAssets
             .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<StaffAssets>> InactiveStaffAsset(CancellationToken cancellationToken = default)
        {
            return await _db.StaffAssets
             .Where(x => x.IsDeleted == true)
             .OrderByDescending(x => x.DeletedDate)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StaffAssets>> SearchStaffAsset(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.StaffAssets
              .Where(x => x.Owner.Contains(keyword.Trim()) || x.Owner.ToLower() == keyword.Trim().ToLower() ||
               x.StoreItemName.Contains(keyword.Trim()) || x.StoreItemName.ToLower() == keyword.Trim().ToLower()) 
              .OrderByDescending(x => x.DateCreated)
              .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(StaffAssets entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
