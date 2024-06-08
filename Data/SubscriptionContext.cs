using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class SubscriptionContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        public SubscriptionContext(DbContextOptions<SubscriptionContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja kluczy głównych
            modelBuilder.Entity<Client>().HasKey(c => c.IdClient);
            modelBuilder.Entity<Subscription>().HasKey(s => s.IdSubscription);
            modelBuilder.Entity<Sale>().HasKey(s => s.IdSale);
            modelBuilder.Entity<Payment>().HasKey(p => p.IdPayment);
            modelBuilder.Entity<Discount>().HasKey(d => d.IdDiscount);

            // Konfiguracja relacji
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.IdClient);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Subscription)
                .WithMany(su => su.Sales)
                .HasForeignKey(s => s.IdSubscription);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.IdClient);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Subscription)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.IdSubscription);

            modelBuilder.Entity<Discount>()
                .HasOne(d => d.Subscription)
                .WithMany(s => s.Discounts)
                .HasForeignKey(d => d.IdSubscription);
        }
    }
}
