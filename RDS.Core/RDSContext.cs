using Microsoft.EntityFrameworkCore;
using RDS.Core.Entities.Tokens;
using RDS.Core.Entities.Users;
using System.Linq;

namespace RDS.Core
{
    public class RDSContext : DbContext
    {
        public RDSContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<User>()
              .ToTable("Users")
              .Ignore(x => x.Gender);

            modelBuilder.Entity<BearerToken>()
            .ToTable("BearerTokens");

            //modelBuilder.Entity<Product>()
            //    .Property(u => u.Name)
            //    .HasMaxLength(100)
            //    .IsRequired();
        }
    }
}
