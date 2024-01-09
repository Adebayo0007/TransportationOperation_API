using Microsoft.EntityFrameworkCore;
using PTS_CORE.Domain.Entities;
using PTS_DATA.EfCore.Context;
using PTS_DATA.Repository.Interfaces;

namespace PTS_DATA.Repository.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDBContext _db;
        public VehicleRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Vehicle entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.vehicles.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.vehicles
              .Where(x => x.IsDeleted == false)
              .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetByIdAsync(string id)
        {
            var result = await _db.vehicles
              .Where(x => x.Id.ToLower() == id.ToLower() || x.RegistrationNumber.ToLower() == id.ToLower())
              .ToListAsync();
            return result ?? null;
        }

        public async Task<Vehicle> GetModelByIdAsync(string id)
        {
            return await _db.vehicles
                .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower() || x.RegistrationNumber.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<Vehicle>> GetTerminalVehicles(string terminalId, CancellationToken cancellationToken = default)
        {
            var result = await _db.vehicles
              .Where(x => x.TerminalId == terminalId)
              .ToListAsync(cancellationToken);
            return result ?? null;
        }

        public async Task<IEnumerable<Vehicle>> InactiveVehicle(CancellationToken cancellationToken = default)
        {
            return await _db.vehicles
              .Where(x => x.IsDeleted == true)
              .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Vehicle entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
