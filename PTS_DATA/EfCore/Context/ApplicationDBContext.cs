﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PTS_CORE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PTS_DATA.EfCore.Context
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDBContext() : base()
        {
        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
        public DbSet<BusBrandingRequest> BusBrandingRequests { get; set; }
        public DbSet<BusBranding> BusBrandings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HireVehicleRequest> HireVehicleRequests { get; set; }
        public DbSet<HireVehicle> HireVehicles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<OtherRequest> OtherRequests { get; set; }
        public DbSet<StoreAsset> StoreAssets { get; set; }
        public DbSet<StoreAssetRequest> StoreAssetRequests { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<StoreItemRequest> StoreItemRequests { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Vehicle> vehicles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.ApplicationRole)
                .WithMany()
                .HasForeignKey(u => u.ApplicationRoleId)
                .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction*/

            /*  modelBuilder.Entity<ApplicationRole>()
                  .HasOne(r => r.ApplicationUser)
                  .WithMany()
                  .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // or DeleteBehavior.NoAction*/
        }


    }
}
