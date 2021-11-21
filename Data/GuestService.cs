using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Data
{
    public class GuestService
    {
        private readonly AppDBContext _context;

        public GuestService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Guest>> GetAll(Guid eventId)
            => await _context.Guests
            .Where(e => e.Event.Id.Equals(eventId))
            .OrderBy(e => e.LastName.ToLower())
            .OrderBy(e => e.Name.ToLower())
            .ToListAsync();

        public async Task<Guest> GetOne(Guid id)
            => await _context.Guests.FirstOrDefaultAsync(t => t.Id.Equals(id));

        public async Task<bool> InsertOne(Guest guest)
        {
            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOne(Guest guest)
        {
            _context.Guests.Update(guest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOne(Guest guest)
        {
            _context.Remove(guest);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
