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
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDBContext _db;
        public SaleRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(Sales entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.Sales.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Sales>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Sales
               .Where(x => x.IsDeleted == false)
               .OrderByDescending(x => x.DateCreated)
               .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Sales>> GetByIdAsync(string id)
        {
            return await _db.Sales
                          .Where(x => x.Id == id)
                          .ToListAsync();
        }

        public async Task<Sales> GetModelByIdAsync(string id)
        {
            return await _db.Sales
                         .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<Sales>> InactiveSaleRequest(CancellationToken cancellationToken = default)
        {
            return await _db.Sales
                          .Where(x => x.IsDeleted == true)
                          .ToListAsync();
        }

        public async Task<IEnumerable<Sales>> SearchSale(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.Sales
          .Where(x => x.Description.Contains(keyword.Trim()) || x.Description.ToLower() == keyword.Trim().ToLower())
          .OrderByDescending(x => x.DateCreated)
          .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Sales entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
