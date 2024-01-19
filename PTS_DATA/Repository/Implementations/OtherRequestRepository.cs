using Microsoft.EntityFrameworkCore;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Interfaces;
using System.Threading;

namespace PTS_DATA.Repository.Implementations
{
    public class OtherRequestRepository : IOtherRequestRepository
    {
        private readonly ApplicationDBContext _db;
        public OtherRequestRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(OtherRequest entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.OtherRequests.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<OtherRequest>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.OtherRequests
              .Where(x => x.IsDeleted == false && x.IsResolved == false)
              .OrderByDescending(x => x.DateCreated)
              .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<OtherRequest>> GetByIdAsync(string id)
        {
            return await _db.OtherRequests
                         .Where(x => x.Id == id)
                         .ToListAsync();
        }

        public async Task<OtherRequest> GetModelByIdAsync(string id)
        {
            return await _db.OtherRequests
                         .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<OtherRequest>> InactivatedOtherRequest(CancellationToken cancellationToken = default)
        {
            return await _db.OtherRequests
              .Where(x => x.IsResolved == true)
              .OrderByDescending(x => x.DateCreated)
              .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<OtherRequest>> MyRequest(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.OtherRequests
           .Where(x => (x.CreatorName.Contains(keyword.Trim()) && x.IsDeleted == false) || (x.CreatorName.ToLower() == keyword.Trim().ToLower() && x.IsDeleted == false))
           .OrderByDescending(x => x.DateCreated)
           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<OtherRequest>> SearchOtherRequests(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.OtherRequests
            .Where(x => x.Subject.Contains(keyword.Trim()) || x.Subject.ToLower() == keyword.Trim().ToLower() ||
            x.Content.Contains(keyword.Trim()) || x.Content.ToLower() == keyword.Trim().ToLower() ||
            x.CreatorName.Contains(keyword.Trim()) || x.CreatorName.ToLower() == keyword.Trim().ToLower())
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(OtherRequest entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
