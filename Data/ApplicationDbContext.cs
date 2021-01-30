using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<SupplierType> SupplierTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanPart> PlanParts { get; set; }
        public DbSet<PlanStep> PlanSteps { get; set; }
        public DbSet<PlanStepSong> PlanStepsSong { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Wedding> Weddings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Guests)
                .WithOne(g => g.Event)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Suppliers)
                .WithOne(s => s.Event)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Attachments)
                .WithOne(a => a.Supplier)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Plan>()
                .HasMany(p => p.Parts)
                .WithOne(p => p.Plan)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlanPart>()
                .HasMany(p => p.Steps)
                .WithOne(s => s.PlanPart)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}