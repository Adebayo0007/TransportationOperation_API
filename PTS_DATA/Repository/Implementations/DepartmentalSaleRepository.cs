﻿using Microsoft.EntityFrameworkCore;
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
    public class DepartmentalSaleRepository : IDepartmentalSaleRepository
    {
        private readonly ApplicationDBContext _db;
        public DepartmentalSaleRepository(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(DepartmentalSale entity)
        {
            if (entity == null) throw new ArgumentNullException();
            var response = await _db.DepartmentalSales.AddAsync(entity);
            if (response.Entity == null) return false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<DepartmentalSale>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.DepartmentalSales
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(x => x.DateCreated)
            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<DepartmentalSale>> GetByIdAsync(string id)
        {
            var result = await _db.DepartmentalSales
          .Where(x => x.Id.ToLower() == id.ToLower())
          .ToListAsync();
            return result ?? null;
        }

        public async Task<DepartmentalSale> GetModelByDepartmentIdAsync(string id)
        {
            return await _db.DepartmentalSales
              .SingleOrDefaultAsync(x => x.IsDeleted == false && x.StartDate.Date.Year == DateTime.Now.Date.Year &&
        x.EndDate.Date.Year == DateTime.Now.Date.Year && x.DepartmentId.ToLower() == id.ToLower());
        }

        public async Task<DepartmentalSale> GetModelByIdAsync(string id)
        {
            return await _db.DepartmentalSales
               .SingleOrDefaultAsync(x => x.Id.ToLower() == id.ToLower());
        }

        public async Task<IEnumerable<DepartmentalSale>> InactiveBudgetTrackings(CancellationToken cancellationToken = default)
        {
            return await _db.DepartmentalSales
        .Where(x => x.IsDeleted == true)
        .OrderByDescending(x => x.DeletedDate)
        .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<DepartmentalSale>> SearchBudgetTrackings(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _db.DepartmentalSales
          .Where(x => x.DepartmentName.Contains(keyword.Trim()) || x.DepartmentName.ToLower() == keyword.Trim().ToLower())
          .OrderByDescending(x => x.DateCreated)
          .ToListAsync(cancellationToken);
        }

        public async Task<decimal> ThisYearBudgetTrackings(string? id,CancellationToken cancellationToken = default)
        {

            return await _db.DepartmentalSales
        .Where(x => x.IsDeleted == false && x.StartDate.Date.Year == DateTime.Now.Date.Year &&
        x.EndDate.Date.Year == DateTime.Now.Date.Year && x.DepartmentId.ToLower() == id.ToLower()).SumAsync(x => x.BudgetedAmount);
        }

        public async Task UpdateAsync(DepartmentalSale entity)
        {
            await _db.SaveChangesAsync();
        }
    }
}
