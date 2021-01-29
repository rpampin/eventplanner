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
            //modelBuilder.Entity<Event>().ToTable("Event");
            //modelBuilder.Entity<EventType>().ToTable("EventType");
            //modelBuilder.Entity<Guest>().ToTable("Guest");
            //modelBuilder.Entity<Program>().ToTable("Program");
            //modelBuilder.Entity<ProgramPart>().ToTable("ProgramPart");
            //modelBuilder.Entity<ProgramStep>().ToTable("ProgramStep");
            //modelBuilder.Entity<ProgramStepSong>().ToTable("ProgramStepSong");
            //modelBuilder.Entity<Supplier>().ToTable("Supplier");
            //modelBuilder.Entity<SupplierType>().ToTable("SupplierType");
            //modelBuilder.Entity<Wedding>().ToTable("Wedding");
        }
    }
}