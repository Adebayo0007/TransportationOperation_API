using Microsoft.EntityFrameworkCore;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Interfaces;
using System.Threading;

namespace PTS_DATA.Repository.Implementations
{
    public class BusBrandingRepository : IBusBrandingRepository
    {
        private readonly ApplicationDBContext _db;
        public BusBrandingRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(BusBranding entity)
        {

            if (entity == null) throw new ArgumentNullException();
            var response = await _db.BusBrandings.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<BusBranding>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.BusBrandings
                .Where(x => x.IsDeleted == false && x.IsApprove == true)
                .OrderByDescending(x => x.LastModified)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BusBranding>> GetByIdAsync(string id)
        {
            return await _db.BusBrandings
             .Where(x => x.Id == id)
             .ToListAsync();
        }

        public async Task<BusBranding> GetModelByIdAsync(string id)
        {
            return await _db.BusBrandings
              .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<BusBranding>> InactiveBranding(CancellationToken cancellationToken = default)
        {
            return await _db.BusBrandings
             .Where(x => x.IsDeleted == true)
             .OrderByDescending(x => x.DeletedDate)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BusBranding>> MarkExpiredBrandAsDeleted()
        {
            return await _db.BusBrandings.Where(x => DateTime.Now.Date > x.BrandEndDate.Date).ToListAsync();
        }

        public async Task<long> NumberOfBrandedVehicle()
        {
            return await _db.BusBrandings
            .Where(x => x.IsDeleted == false)
            .SumAsync(x => x.NumberOfVehicle);

        }

        public async Task<IEnumerable<BusBranding>> SearchBranding(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.BusBrandings
             .Where(x => x.PartnerName.ToLower() == keyword.ToLower() || x.PartnerName.Contains(keyword))
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BusBranding>> UnApprovedBranding(CancellationToken cancellationToken = default)
        {
            return await _db.BusBrandings
                .Where(x => x.IsDeleted == false && x.IsApprove == false)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(BusBranding entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
