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
    public class TerminalRepository : ITerminalRepository
    {
        private readonly ApplicationDBContext _db;
        public TerminalRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Terminal entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Terminals.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Terminal>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Terminals
              .Where(x => x.IsDeleted == false)
              .OrderByDescending(x => x.DateCreated)
              .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Terminal>> GetByIdAsync(string id)
        {
            var result = await _db.Terminals
            .Where(x => x.Id.ToLower() == id.ToLower())
            .ToListAsync();
            return result ?? null;
        }

        public async Task<Terminal> GetModelByIdAsync(string id)
        {
            return await _db.Terminals
               .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<Terminal> GetModelByNameAsync(string name)
        {
            return await _db.Terminals
               .SingleOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Terminal>> InactiveTerminal(CancellationToken cancellationToken = default)
        {
            return await _db.Terminals
             .Where(x => x.IsDeleted == true)
             .OrderByDescending(x => x.DeletedDate)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Terminal>> SearchTerminal(string? keyword = null, CancellationToken cancellationToken = default)
        {
            return await _db.Terminals
             .Where(x => x.Name.ToLower().Contains(keyword.Trim().ToLower()) || x.Code.Contains(keyword.Trim()) || x.State.Contains(keyword.Trim()) || x.State.ToLower() == keyword.Trim().ToLower())
             .OrderByDescending(x => x.DateCreated)
             .ToListAsync(cancellationToken); 
        }

        public async Task UpdateAsync(Terminal entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
