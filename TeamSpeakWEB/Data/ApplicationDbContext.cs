using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamSpeakWEB.Models;

namespace TeamSpeakWEB.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                    .HasMany(u => u.tsservers)
                    .WithOne(ts => ts.user)
                    .IsRequired();

            builder.Entity<User>()
                .HasOne(u => u.referalFrom)
                .WithMany(u => u.refUsers)
                .IsRequired(false);

            base.OnModelCreating(builder);
        }

        public DbSet<Tsserver> Tsservers { get; set; }
    }
}
