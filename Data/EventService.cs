using EventPlanner.Models;
using EventPlanner.Models.ModelView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanner.Data
{
    public class EventService
    {
        private readonly AppDBContext _context;

        public EventService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventListView>> GetAll()
            => await Get(_context.BaseEvents.AsQueryable());

        public async Task<IEnumerable<EventListView>> GetAllPending()
            => await Get(_context.BaseEvents.Where(e => e.Date >= DateTime.Now.Date));

        async Task<IEnumerable<EventListView>> Get(IQueryable<BaseEvent> query)
        {
            var events = new List<EventListView>();

            var baseEvents = await query
                .Include(e => e.Type)
                .Include(e => e.Package)
                .Include(e => e.Guests)
                .Include(e => e.Suppliers)
                .OrderBy(e => e.Date)
                .ToListAsync();

            foreach (var e in baseEvents)
            {
                events.Add(new EventListView
                {
                    Id = e.Id,
                    Date = e.Date,
                    Type = e.Type.Name,
                    Celebrant = e is Wedding ? JoinData(" - ", new string[] { (e as Wedding).BrideName, (e as Wedding).GroomName }) : (e as Event).Celebrant,
                    Address = e is Wedding ? JoinData(" - ", new string[] { (e as Wedding).BrideAddress, (e as Wedding).GroomAddress }) : (e as Event).Address,
                    Mobile = e is Wedding ? JoinData(" - ", new string[] { (e as Wedding).BrideMobile, (e as Wedding).GroomMobile }) : (e as Event).Mobile,
                    Email = e is Wedding ? JoinData(" - ", new string[] { (e as Wedding).BrideEmail, (e as Wedding).GroomEmail }) : (e as Event).Email,
                    Package = e.Package,
                    DownPayment = e.PackagePrice,
                    Balance = e.Balance,
                    GuestsCount = e.Guests.Count(),
                    SuppliersCount = e.Suppliers.Count()
                });
            }

            return events;
        }

        string JoinData(string separator, string[] data)
        {
            data = data.Where(d => !string.IsNullOrWhiteSpace(d)).ToArray();
            return string.Join(separator, data);
        }

        public async Task<Guid> GetOneType(Guid id)
            => await _context.BaseEvents
                .Where(e => e.Id == id)
                .Select(e => e.Type.Id)
                .SingleAsync();

        public async Task<T> GetOne<T>(Guid id)
            where T : BaseEvent
            => await _context.Set<T>()
                .Include(e => e.Type)
                .Include(e => e.Attachments)
                .Include(e => e.Guests)
                .Include(e => e.Suppliers)
                .FirstOrDefaultAsync(t => t.Id.Equals(id));

        public async Task<EventDataView> GetOneData(Guid id)
        {
            var ev = await _context
                .BaseEvents
                .Include(e => e.Type)
                .Where(e => e.Id == id)
                .SingleAsync();
            
            var data = new EventDataView
            {
                Type = ev.Type.Name,
                Date = ev.Date.Value
            };

            if (ev is Event)
                data.Celebrants = (ev as Event).Celebrant;
            else
                data.Celebrants = (ev as Wedding).BrideName + " - " + (ev as Wedding).GroomName;

            return data;
        }

        public async Task<bool> InsertOne<T>(T Event)
            where T : BaseEvent
        {
            await _context.AddAsync(Event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WeddingInsertOne(Wedding Event)
        {
            await _context.BaseEvents.AddAsync(Event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOne(BaseEvent Event)
        {
            _context.BaseEvents.Update(Event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WeddingUpdateOne(Wedding Event)
        {
            _context.BaseEvents.Update(Event);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOne(Guid Id)
        {
            var @event = await _context.BaseEvents.FindAsync(Id);
            _context.Remove(@event);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
