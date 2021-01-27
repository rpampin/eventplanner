using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<SupplierType> SupplierTypes { get; set; }
        
        //public DbSet<Event> Events { get; set; }
        //public DbSet<Guest> Guests { get; set; }
        //public DbSet<Program> Programs { get; set; }
        //public DbSet<ProgramPart> ProgramParts { get; set; }
        //public DbSet<ProgramStep> ProgramSteps { get; set; }
        //public DbSet<ProgramStepSong> ProgramStepsSong { get; set; }
        //public DbSet<Supplier> Suppliers { get; set; }
        //public DbSet<Wedding> Weddings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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