﻿using Domain;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options ) : base(options){}

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Dept> Depts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(x => x.Purchases)
                .WithOne(q => q.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                ;

            builder.Entity<AppUser>()
                .HasMany(a => a.Transfers)
                .WithOne(q => q.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                ;

            //builder.Entity<AppUser>()
            //    .HasMany(x => x.Folders)
            //    .WithOne(x => x.User)
            //    .HasForeignKey(x => x.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasMany(x => x.Depts)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Purchase>()
                .HasMany(a => a.Invoice)
                .WithOne(q => q.Purchase)
                .HasForeignKey(w => w.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);



        }

    }
}
