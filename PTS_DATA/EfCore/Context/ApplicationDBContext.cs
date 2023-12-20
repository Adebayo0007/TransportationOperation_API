using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        //public DbSet<Employee> Employees { get; set; }
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
