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
    public class ComplainRepository : IComplainRepository
    {
        private readonly ApplicationDBContext _db;
        public ComplainRepository(ApplicationDBContext db)
        {
            _db = db;
        }

         public async Task<bool> CreateAsync(Complain entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Complains.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Complain>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Complains
                 .Where(x => x.IsDeleted == false)
                 .OrderByDescending(x => x.DateCreated)
                 .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Complain>> GetByIdAsync(string id)
        {
            return await _db.Complains
                .Where(x => x.Id == id)
                .ToListAsync();
        }

        public async Task<Complain> GetModelByIdAsync(string id)
        {
            return await _db.Complains
               .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Complain>> InactiveComplains(CancellationToken cancellationToken = default)
        {
            return await _db.Complains
              .Where(x => x.IsDeleted == true)
              .OrderByDescending(x => x.DeletedDate)
              .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Complain>> SearchComplains(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.Complains
              .Where(x => x.CreatorName.ToLower() == keyword.ToLower() || x.CreatorName.Contains(keyword))
              .OrderByDescending(x => x.DateCreated)
              .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Complain entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
