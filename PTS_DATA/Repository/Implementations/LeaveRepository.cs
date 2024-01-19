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
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDBContext _db;
        public LeaveRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Leave entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Leaves.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Leave>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Leaves
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync(cancellationToken);
        }

     

        public async Task<IEnumerable<Leave>> GetByIdAsync(string id)
        {
            return await _db.Leaves
               .Where(x => x.Id == id)
               .ToListAsync();
        }

        public async Task<Leave> GetModelByIdAsync(string id)
        {
            return await _db.Leaves
               .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Leave>> InactiveLeaves(CancellationToken cancellationToken = default)
        {
            return await _db.Leaves
               .Where(x => x.IsDeleted == true)
               .OrderByDescending(x => x.DeletedDate)
               .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Leave>> SearchLeaves(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.Leaves
              .Where(x => x.CreatorName.ToLower() == keyword.ToLower() || x.CreatorName.Contains(keyword))
              .OrderByDescending(x => x.DateCreated)
              .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Leave entity)
        {
            await _db.SaveChangesAsync();
        }

     
    }
}
