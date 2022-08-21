using Domain;

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
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Dept> Depts { get; set; }
        public DbSet<Payment> Payments { get; set; }


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

            builder.Entity<AppUser>()
                .HasMany(x => x.Folders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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

            builder.Entity<Folder>()
                .HasMany(x => x.Depts)
                .WithOne(x => x.Folder)
                .HasForeignKey(x => x.FolderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Folder>()
                .HasMany(x => x.Purchases)
                .WithOne(x => x.Folder)
                .HasForeignKey(x => x.FolderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Folder>()
                .HasMany(x => x.Transfers)
                .WithOne(x => x.Folder)
                .HasForeignKey(x => x.FolderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Dept>()
                .HasMany(x => x.Payments)
                .WithOne(x => x.Dept)
                .HasForeignKey(x => x.DeptId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Dept>()
                .HasOne(x => x.Folder)
                .WithMany(x => x.Depts)
                .HasForeignKey(x => x.FolderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payment>()
                .HasOne(x => x.Dept)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.DeptId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
