using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;

namespace RaceDay.API.Data
{
    public class RaceDayContext : DbContext
    {
        public RaceDayContext(
            DbContextOptions<RaceDayContext> options)
            : base(options)
        {
        }

        // Database tables
        public DbSet<User> Users => Set<User>();
        public DbSet<RaceDay.API.Models.Organiser> Organisers => Set<RaceDay.API.Models.Organiser>();
        public DbSet<Participant> Participants => Set<Participant>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<Result> Results => Set<Result>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USERS
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // USER - ORGANISER (one-to-zero-or-one)
            modelBuilder.Entity<Organiser>()
                .HasIndex(o => o.UserID)
                .IsUnique();

            modelBuilder.Entity<Organiser>()
                .HasOne(o => o.User)
                .WithOne(static u => u.Organiser)
                .HasForeignKey<Organiser>(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // USER - PARTICIPANT (one-to-zero-or-one)
            modelBuilder.Entity<Participant>()
                .HasIndex(p => p.UserID)
                .IsUnique();

            modelBuilder.Entity<Participant>()
                .HasOne(p => p.User)
                .WithOne(u => u.Participant)
                .HasForeignKey<Participant>(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // ORGANISER - EVENTS (one-to-many)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organiser)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.Restrict);

            // EVENT - CATEGORIES (one-to-many)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Event)
                .WithMany(e => e.Categories)
                .HasForeignKey(c => c.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            // Avoid duplicate category names within one event.
            modelBuilder.Entity<Category>()
                .HasIndex(c => new { c.EventID, c.CategoryName })
                .IsUnique();

            // PARTICIPANT - ENROLLMENTS (one-to-many)
            modelBuilder.Entity<Enrollment>()
                .HasOne(en => en.Participant)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(en => en.ParticipantID)
                .OnDelete(DeleteBehavior.Restrict);

            // EVENT - ENROLLMENTS (one-to-many)
            modelBuilder.Entity<Enrollment>()
                .HasOne(en => en.Event)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(en => en.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            // CATEGORY - ENROLLMENTS (one-to-many)
            modelBuilder.Entity<Enrollment>()
                .HasOne(en => en.Category)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(en => en.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            // RESULT - ENROLLMENT (one-to-zero-or-one)
            modelBuilder.Entity<Result>()
                .HasIndex(r => r.EnrollmentID)
                .IsUnique();

            modelBuilder.Entity<Result>()
                .HasOne(r => r.Enrollment)
                .WithOne(en => en.Result)
                .HasForeignKey<Result>(r => r.EnrollmentID)
                .OnDelete(DeleteBehavior.Cascade);

            // Store race distances using decimal precision.
            modelBuilder.Entity<Event>()
                .Property(e => e.Distance)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Category>()
                .Property(c => c.MinimumDistance)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Category>()
                .Property(c => c.MaximumDistance)
                .HasPrecision(8, 2);
        }
    }
}
