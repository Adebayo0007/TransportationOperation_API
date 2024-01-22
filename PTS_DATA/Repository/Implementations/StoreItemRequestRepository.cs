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
    public class StoreItemRequestRepository : IStoreItemRequestRepository
    {
        private readonly ApplicationDBContext _db;
        public StoreItemRequestRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(StoreItemRequest entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.StoreItemRequests.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<StoreItemRequest>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.StoreItemRequests
           .Where(x => x.IsResolved == true)
           .OrderByDescending(x => x.LastModified)
           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItemRequest>> GetAllForAuditor(CancellationToken cancellationToken = default)
        {
            return await _db.StoreItemRequests
          .Where(x => x.IsDeleted == false && (int)x.AvailabilityType == 1 && x.IsResolved == false && x.IsAuditorCommented == false)
          .OrderByDescending(x => x.DateCreated)
          .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItemRequest>> GetAllForChirman(CancellationToken cancellationToken = default)
        {
            return await _db.StoreItemRequests
         .Where(x => x.IsDeleted == false && (int)x.AvailabilityType == 1 &&
         x.IsAuditorCommented == true && x.AuditorComment != null && x.IsDDPCommented == true && x.IsResolved == false)
         .OrderByDescending(x => x.DateCreated)
         .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItemRequest>> GetAllForDDP(CancellationToken cancellationToken = default)
        {
            return await _db.StoreItemRequests
          .Where(x => x.IsDeleted == false && (int)x.AvailabilityType == 1 && x.IsAuditorCommented == true && x.IsResolved == false && x.IsDDPCommented == false)
          .OrderByDescending(x => x.DateCreated)
          .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItemRequest>> GetAllForStore(CancellationToken cancellationToken = default)
        {
            return await _db.StoreItemRequests
           .Where(x => x.IsDeleted == false && x.AvailabilityType == null && x.IsResolved == false)
           .OrderByDescending(x => x.DateCreated)
           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItemRequest>> GetByIdAsync(string id)
        {
            var result = await _db.StoreItemRequests
              .Where(x => x.Id.ToLower() == id.ToLower())
              .ToListAsync();
            return result ?? null;
        }

        public async Task<StoreItemRequest> GetModelByIdAsync(string id)
        {
            return await _db.StoreItemRequests
            .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<StoreItemRequest>> InactiveStoreItemRequest(CancellationToken cancellationToken = default)
        {
            return await _db.StoreItemRequests
            .Where(x => x.IsDeleted == true)
            .OrderByDescending(x => x.DeletedDate)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItemRequest>> MystoreItemRequest(string id, CancellationToken cancellationToken = default)
        {
            return await _db.StoreItemRequests
           .Where(x => x.IsDeleted == false && x.CreatorId == id)
           .OrderByDescending(x => x.DateCreated)
           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<StoreItemRequest>> SearchStoreItemRequest(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.StoreItemRequests
          .Where(x => x.StoreItemName.Contains(keyword.Trim()) || x.StoreItemName.ToLower() == keyword.Trim().ToLower() ||
          x.TerminalName.Contains(keyword.Trim()) || x.TerminalName.ToLower() == keyword.Trim().ToLower() ||
          x.VehicleRegistrationNumber.Contains(keyword.Trim()) || x.VehicleRegistrationNumber.ToLower() == keyword.Trim().ToLower() ||
           x.CreatorName.Contains(keyword.Trim()) || x.CreatorName.ToLower() == keyword.Trim().ToLower())
          .OrderByDescending(x => x.DateCreated)
          .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(StoreItemRequest entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
