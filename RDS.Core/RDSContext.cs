using Microsoft.EntityFrameworkCore;
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

            //modelBuilder.Entity<User>()
            //  .ToTable("User");

            //modelBuilder.Entity<User>()
            //    .Property(u => u.Name)
            //    .HasMaxLength(100)
            //    .IsRequired();
        }
    }
}
