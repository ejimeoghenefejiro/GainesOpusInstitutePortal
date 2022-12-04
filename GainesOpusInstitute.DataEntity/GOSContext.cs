using GainesOpusInstitute.DataEntity.Entity;
using GainesOpusInstitute.DataLayer.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainesOpusInstitute.DataEntity
{
    public class GOSContext : IdentityDbContext<User, Role, int>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public GOSContext(DbContextOptions<GOSContext> options, IHttpContextAccessor contextAccessor) : base(options)
        {
            _contextAccessor = contextAccessor;
        }
        public GOSContext(DbContextOptions options) : base(options)
        {

        } 
        public DbSet<Profile> profiles { get; set; }
        public DbSet<Pricing> prices { get; set; }
        public DbSet<TeacherAvailablity> TeacherAvailablities { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<User>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Role>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Role>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int saved = 0;
            var currentUser = _contextAccessor.HttpContext.User.Identity.Name;
            var currentDate = DateTime.Now;
            try
            {
                foreach (var entry in ChangeTracker.Entries<BaseEnities>()
               .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.DateCreated = currentDate;
                        entry.Entity.CreatedBy = currentUser;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastDateUpdated = currentDate;
                        entry.Entity.UpdatedBy = currentUser;
                    }
                }
                saved = await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception Ex)
            {
                //Log.Error(Ex, "An error has occured in SaveChanges");
                //Log.Error(Ex.InnerException, "An error has occured in SaveChanges InnerException");
                //Log.Error(Ex.StackTrace, "An error has occured in SaveChanges StackTrace");
            }

            return saved;
            //   return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            int saved = 0;
            var currentUser = _contextAccessor.HttpContext.User.Identity.Name;
            var currentDate = DateTime.Now;
            try
            {
                foreach (var entry in ChangeTracker.Entries<BaseEnities>()
               .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.DateCreated = currentDate;
                        entry.Entity.CreatedBy = currentUser;
                        //entry.Entity.LastDateUpdated = currentDate;
                        //entry.Entity.UpdatedBy = currentUser;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastDateUpdated = currentDate;
                        entry.Entity.UpdatedBy = currentUser;
                        //if (entry.Entity.IsDeleted == true && entry.Entity.DateDeleted == null)
                        //{
                        //    entry.Entity.DateDeleted = currentDate;
                        //    entry.Entity.DeletedBy = currentUser;
                        //}
                    }
                }
                saved = base.SaveChanges();
            }
            catch (Exception Ex)
            {
                //Log.Error(Ex, "An error has occured in SaveChanges");
                //Log.Error(Ex.InnerException, "An error has occured in SaveChanges InnerException");
                //Log.Error(Ex.StackTrace, "An error has occured in SaveChanges StackTrace");
                throw Ex;
            }

            return saved;
        }
        public class ContextDesignFactory : IDesignTimeDbContextFactory<GOSContext>
        {
            public GOSContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<GOSContext>()
                                //.UseSqlServer("Data Source=TEMP053-SOFT-01\\SQLEXPRESS;Initial Catalog=MIA_RegPortal;Integrated Security=True");
                                .UseSqlServer("Server=.;Database=GOS.Data;Trusted_Connection=True;MultipleActiveResultSets=true");

                return new GOSContext(optionsBuilder.Options);
            }
        }
    }
}
