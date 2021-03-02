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
                new EventType { Name = "Corporations" }
                };

                context.EventTypes.AddRange(eventTypes);
            }

            if (!context.SupplierTypes.Any())
            {
                var supplierTypes = new SupplierType[]
                {
                    new SupplierType { Name = "BRIDAL GOWN & ACCESSORIES" },
                    new SupplierType { Name = "GROM SUIT" },
                    new SupplierType { Name = "ENTOURAGE GOWNS AND SUIT" },
                    new SupplierType { Name = "BRIDAL CAR" },
                    new SupplierType { Name = "CATERER" },
                    new SupplierType { Name = "CAKE" },
                    new SupplierType { Name = "DOVE IN CAGE" },
                    new SupplierType { Name = "WEDDING RING" },
                    new SupplierType { Name = "CEREMONY ARRANGEMENT" },
                    new SupplierType { Name = "ENTOURAGE FLOWERS" },
                    new SupplierType { Name = "RECEPTION STYLING" },
                    new SupplierType { Name = "HAIR AND MAKE UP FOR BRIDE" },
                    new SupplierType { Name = "HAIR AND MAKE UP FOR ENTOURAGE" },
                    new SupplierType { Name = "PRENUP STYLING HMU" },
                    new SupplierType { Name = "ENTERTAINMENT CHURCH/RECEPTION" },
                    new SupplierType { Name = "LCD PROJECTOR & SCREEN" },
                    new SupplierType { Name = "EMCEE" },
                    new SupplierType { Name = "LIGHTS & SOUND SYSTEM" },
                    new SupplierType { Name = "LED WALL" },
                    new SupplierType { Name = "PHOTOGRAPHER" },
                    new SupplierType { Name = "VIDEOGRAPHER" },
                    new SupplierType { Name = "PHOTOBOOTH" },
                    new SupplierType { Name = "INVITATION CARDS/INVITES" },
                    new SupplierType { Name = "MISALETTE/PROGRAM" },
                    new SupplierType { Name = "STAGE/DANCE FLOOR" },
                    new SupplierType { Name = "FIREWORKS" },
                    new SupplierType { Name = "POPPERS" },
                    new SupplierType { Name = "CREW MEAL" }
                };

                context.SupplierTypes.AddRange(supplierTypes);
            }

            context.SaveChanges();
        }
    }
}