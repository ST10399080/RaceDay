using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;

namespace RaceDay.API.Data;

public class RaceDayDbContext : DbContext
{
    public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<Result> Results => Set<Result>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User: unique email address
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // User: Participant profile (one-to-zero-or-one)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Participant)
            .WithOne(p => p.User)
            .HasForeignKey<Participant>(p => p.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Participant>()
            .HasIndex(p => p.UserID)
            .IsUnique();

        // User: Organiser -> Events (one-to-many)
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Organiser)
            .WithMany(u => u.OrganisedEvents)
            .HasForeignKey(e => e.OrganiserID)
            .OnDelete(DeleteBehavior.Restrict);

        // Event -> Categories (one-to-many)
        modelBuilder.Entity<Category>()
            .HasOne(c => c.Event)
            .WithMany(e => e.Categories)
            .HasForeignKey(c => c.EventID)
            .OnDelete(DeleteBehavior.Cascade);

        // Participant -> Enrolments (one-to-many)
        modelBuilder.Entity<Enrolment>()
            .HasOne(e => e.Participant)
            .WithMany(p => p.Enrolments)
            .HasForeignKey(e => e.ParticipantID)
            .OnDelete(DeleteBehavior.Restrict);

        // Event -> Enrolments (one-to-many)
        modelBuilder.Entity<Enrolment>()
            .HasOne(e => e.Event)
            .WithMany(e => e.Enrolments)
            .HasForeignKey(e => e.EventID)
            .OnDelete(DeleteBehavior.Restrict);

        // Category -> Enrolments (one-to-many)
        modelBuilder.Entity<Enrolment>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Enrolments)
            .HasForeignKey(e => e.CategoryID)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent a participant enrolling in the same event twice
        modelBuilder.Entity<Enrolment>()
            .HasIndex(e => new { e.ParticipantID, e.EventID })
            .IsUnique();

        // Enrolment -> Result (one-to-zero-or-one)
        modelBuilder.Entity<Enrolment>()
            .HasOne(e => e.Result)
            .WithOne(r => r.Enrolment)
            .HasForeignKey<Result>(r => r.EnrolmentID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Result>()
            .HasIndex(r => r.EnrolmentID)
            .IsUnique();

        // Database precision for event distance
        modelBuilder.Entity<Event>()
            .Property(e => e.Distance)
            .HasPrecision(8, 2);
    }
}
