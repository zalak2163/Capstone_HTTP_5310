using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection.Emit;
using EventPlanningCapstoneProject.Models;


namespace EventPlanningCapstoneProject.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Analytics> Analytics { get; set; }
        public DbSet<EventCalendar> EventCalendars { get; set; }
        public DbSet<SocialShare> SocialShares { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Purchase <-> User Relationship
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.User)  // Purchase has one User
                .WithMany()  // User can have many Purchases (but no navigation property in User class)
                .HasForeignKey(p => p.UserId)  // Define foreign key as UserId
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            // Configure Purchase <-> Event Relationship
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Event)  // Purchase has one Event
                .WithMany()  // Event can have many Purchases (but no navigation property in Event class)
                .HasForeignKey(p => p.EventId)  // Define foreign key as EventId
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            // Configure Purchase <-> Ticket Relationship
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Ticket)  // Purchase has one Ticket
                .WithMany()  // Ticket can have many Purchases (but no navigation property in Ticket class)
                .HasForeignKey(p => p.TicketId)  // Define foreign key as TicketId
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete

            // Define the precision for decimal fields
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Purchase>()
                .Property(p => p.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");
        }



    }
}
