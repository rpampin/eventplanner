using EventPlanner.Models;
using EventPlanner.Models.ModelView;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Data
{
    public class EventService
    {
        private readonly AppDBContext _context;

        public EventService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EvenListView>> GetAll()
            => await Get(_context.Events.AsQueryable());

        public async Task<IEnumerable<EvenListView>> GetAllPending()
            => await Get(_context.Events.Where(e => e.Date >= DateTime.Now.Date));


        async Task<IEnumerable<EvenListView>> Get(IQueryable<Event> query)
            => await query
            .OrderBy(e => e.Date)
            .Select(e => new EvenListView
            {
                Id = e.Id,
                Date = e.Date,
                Type = e.Type.Name,
                Celebrant = e.Celebrant,
                Address = e.Address,
                Mobile = e.Mobile,
                Email = e.Email,
                Package = e.Package,
                DownPayment = e.PackagePrice,
                Balance = e.Balance,
                GuestsCount = e.Guests.Count(),
                SuppliersCount = e.Suppliers.Count()
            })
            .ToListAsync();

        public async Task<Event> GetOne(Guid id)
            => await _context.Events
                .Include(e => e.Type)
                .Include(e => e.Attachments)
                .FirstOrDefaultAsync(t => t.Id.Equals(id));

        public async Task<bool> InsertOne(Event Event)
        {
            await _context.Events.AddAsync(Event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WeddingInsertOne(Wedding Event)
        {
            await _context.Events.AddAsync(Event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOne(Event Event)
        {
            _context.Events.Update(Event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WeddingUpdateOne(Wedding Event)
        {
            _context.Events.Update(Event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOne(Guid Id)
        {
            var @event = await _context.Events.FindAsync(Id);
            _context.Remove(@event);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
