using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RDS.Core.Entities.Base;
using RDS.Core.Entities.Tokens;
using RDS.Core.Entities.Users;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RDS.Core
{
    public class RDSContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RDSContext(DbContextOptions<RDSContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region Dbset
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ActionName> ActionNames { get; set; }
        public DbSet<ActionNamePermission> ActionNamePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        #endregion


        #region ModelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            modelBuilder.Entity<BearerToken>().HasKey(bt => bt.Id);

            #region User -Role
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.RoleId, ur.UserId });

            modelBuilder.Entity<UserRole>()
                .HasOne<User>(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<User>().Ignore(x => x.Gender);


            modelBuilder.Entity<UserRole>()
                .HasOne<Role>(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            #endregion
            #region Role - Permission
            modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne<Role>(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne<Permission>(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            #endregion

            #region ActionName - Permission
            modelBuilder.Entity<ActionNamePermission>().HasKey(anp => new { anp.PermissionId, anp.ActionNameId });

            modelBuilder.Entity<ActionNamePermission>()
                .HasOne<ActionName>(anp => anp.ActionName)
                .WithMany(an => an.ActionNamePermissions)
                .HasForeignKey(anp => anp.ActionNameId);


            modelBuilder.Entity<ActionNamePermission>()
               .HasOne<Permission>(anp => anp.Permission)
               .WithMany(p => p.ActionNamePermissions)
               .HasForeignKey(anp => anp.PermissionId);
            #endregion

        }

        #endregion

        #region Override

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateAuditEntities()
        {
            DateTime now = DateTime.UtcNow;
            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            var modifiedEntries = ChangeTracker.Entries().Where(x =>
                x.Entity is BaseEntity &&
                (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                    entity.CreatedBy = userName;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                }

                entity.UpdatedAt = now;
                entity.UpdatedBy = userName;
            }
        }

        #endregion
    }
}
