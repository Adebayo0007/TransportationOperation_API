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
    public class StoreAssetRepository : IStoreAssetRepository
    {
        private readonly ApplicationDBContext _db;
        public StoreAssetRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(StoreAsset entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.StoreAssets.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<StoreAsset>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.StoreAssets
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreAsset>> GetByIdAsync(string id)
        {
            var result = await _db.StoreAssets
             .Where(x => x.Id.ToLower() == id.ToLower())
             .ToListAsync();
            return result ?? null;
        }

        public async Task<StoreAsset> GetModelByIdAsync(string id)
        {
            return await _db.StoreAssets
             .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<StoreAsset> GetStoreAssetByTerminalIdAndStoreItemid(string terminalId, string storeItemId)
        {
            return await _db.StoreAssets
             .SingleOrDefaultAsync(x => x.TerminalId.ToLower() == terminalId.ToLower() && x.StoreItemId.ToLower() == storeItemId);
        }

        public async Task<IEnumerable<StoreAsset>> InactiveStoreAsset(CancellationToken cancellationToken = default)
        {
            return await _db.StoreAssets
            .Where(x => x.IsDeleted == true)
            .OrderByDescending(x => x.DeletedDate)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreAsset>> SearchStoreAsset(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.StoreAssets
           .Where(x => x.StoreItemName.Contains(keyword.Trim()) || x.StoreItemName.ToLower() == keyword.Trim().ToLower()||
           x.TerminalName.Contains(keyword.Trim()) || x.TerminalName.ToLower() == keyword.Trim().ToLower() ||
           x.TerminalId.Contains(keyword.Trim()) || x.TerminalId.ToLower() == keyword.Trim().ToLower())
           .OrderByDescending(x => x.DateCreated)
           .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(StoreAsset entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
