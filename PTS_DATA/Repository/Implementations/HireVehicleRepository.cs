using PTS_DATA.Repository.Interfaces;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using Microsoft.EntityFrameworkCore;

namespace PTS_DATA.Repository.Implementations
{
    public class HireVehicleRepository : IHireVehicleRepository
    {
        private readonly ApplicationDBContext _db;
        public HireVehicleRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(HireVehicle entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.HireVehicles.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<HireVehicle>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.HireVehicles
            .Where(x => x.IsDeleted == false && x.IsChairmanApprove == true && x.ResolvedByOperation == true && x.ResolvedByDepo == true)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<HireVehicle>> GetAllForChairman(CancellationToken cancellationToken = default)
        {
            return await _db.HireVehicles
           .Where(x => x.IsDeleted == false && x.IsChairmanApprove == false && x.ResolvedByOperation == false && x.ResolvedByDepo == false)
           .OrderByDescending(x => x.DateCreated)
           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<HireVehicle>> GetAllForDepo(CancellationToken cancellationToken = default)
        {
            return await _db.HireVehicles
          .Where(x => x.IsDeleted == false && x.IsChairmanApprove == true && x.ResolvedByOperation == true && x.ResolvedByDepo == false)
          .OrderByDescending(x => x.DateCreated)
          .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<HireVehicle>> GetAllForOperation(CancellationToken cancellationToken = default)
        {
            return await _db.HireVehicles
           .Where(x => x.IsDeleted == false && x.IsChairmanApprove == true && x.ResolvedByOperation == false && x.ResolvedByDepo == false)
           .OrderByDescending(x => x.DateCreated)
           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<HireVehicle>> GetByIdAsync(string id)
        {
            var result = await _db.HireVehicles
           .Where(x => x.Id.ToLower() == id.ToLower())
           .ToListAsync();
            return result ?? null;
        }

        public async Task<HireVehicle> GetModelByIdAsync(string id)
        {
            return await _db.HireVehicles
              .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<HireVehicle>> InactiveHiredVeehicle(CancellationToken cancellationToken = default)
        {
            return await _db.HireVehicles
            .Where(x => x.IsDeleted == true)
            .OrderByDescending(x => x.DeletedDate)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<HireVehicle>> SearchHiredVehicles(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.HireVehicles
             .Where(x => x.Customer.Contains(keyword.Trim()) || x.Customer.ToLower() == keyword.Trim().ToLower())
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<HireVehicle>> UnApprovedHiring(CancellationToken cancellationToken = default)
        {
            return await _db.HireVehicles
          .Where(x => x.IsDeleted == false && x.IsChairmanApprove == false)
          .OrderByDescending(x => x.DateCreated)
          .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(HireVehicle entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
