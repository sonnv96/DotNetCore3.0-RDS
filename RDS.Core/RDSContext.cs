using Microsoft.EntityFrameworkCore;
using RDS.Core.Entities;
using RDS.Core.Entities.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
