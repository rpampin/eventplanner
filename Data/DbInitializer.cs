using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EventPlanner.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            context.Database.Migrate();

            // Look for any students.
            if (!context.EventTypes.Any())
            {
                var eventTypes = new EventType[]
                {
                    new EventType { Name = "Wedding" },
                    new EventType { Name = "Birthday" },
                    new EventType { Name = "Debut" },
                    new EventType { Name = "Corporate" },
                    new EventType { Name = "Reunion" },
                    new EventType { Name = "Birthday" },
                    new EventType { Name = "Corporate" },
                    new EventType { Name = "Retirement" }
                };

                context.EventTypes.AddRange(eventTypes);
            }

            if (!context.SupplierTypes.Any())
            {
                var supplierTypes = new SupplierType[]
                {
                    new SupplierType { Name = "Venue" },
                    new SupplierType { Name = "Caterer" },
                    new SupplierType { Name = "Florist" },
                    new SupplierType { Name = "Stylist" },
                    new SupplierType { Name = "Emcee" },
                    new SupplierType { Name = "Band" },
                    new SupplierType { Name = "Photographer" },
                    new SupplierType { Name = "Videographer" },
                    new SupplierType { Name = "Photobooth" },
                    new SupplierType { Name = "Crew meals" },
                    new SupplierType { Name = "Jeweler" },
                    new SupplierType { Name = "Skin Aesthetics" },
                    new SupplierType { Name = "Teeth Aesthetics" },
                    new SupplierType { Name = "Preparation Venue" },
                    new SupplierType { Name = "Couture" },
                    new SupplierType { Name = "Tailor" },
                    new SupplierType { Name = "Lights & Sounds" },
                    new SupplierType { Name = "LED Wall" },
                    new SupplierType { Name = "Screen & Projector" },
                    new SupplierType { Name = "LED Animation" },
                    new SupplierType { Name = "Poppers / Confetti" },
                    new SupplierType { Name = "Fireworks" },
                    new SupplierType { Name = "Bridal Car" },
                    new SupplierType { Name = "DJ" },
                    new SupplierType { Name = "Mobile Bar" },
                    new SupplierType { Name = "Cake" },
                    new SupplierType { Name = "Lechon" },
                    new SupplierType { Name = "Beef Carving Station" },
                    new SupplierType { Name = "Transport" },
                    new SupplierType { Name = "Grazing Table / Box" },
                    new SupplierType { Name = "Groom groomer" },
                    new SupplierType { Name = "Invitation Printing" },
                    new SupplierType { Name = "Souvenir for Sponsors" },
                    new SupplierType { Name = "Souvenir - General" },
                    new SupplierType { Name = "Technical director" }
                };

                context.SupplierTypes.AddRange(supplierTypes);
            }

            context.SaveChanges();
        }
    }
}