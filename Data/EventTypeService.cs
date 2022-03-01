using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanner.Data
{
    public class EventTypeService
    {
        private readonly AppDBContext _context;

        public EventTypeService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Guid> GetWeddingEventTypeId()
            => await _context.EventTypes
                .Where(e => e.Name.ToLower() == "wedding")
                .Select(e => e.Id)
                .SingleAsync();

        public async Task<IEnumerable<EventType>> GetAll()
            => await _context.EventTypes
            .OrderBy(e => e.Name.ToLower())
            .ToListAsync();

        public async Task<EventType> GetOne(Guid id)
            => await _context.EventTypes.FirstOrDefaultAsync(t => t.Id.Equals(id));
        
        public async Task<bool> InsertOne(EventType eventType)
        {
            await _context.EventTypes.AddAsync(eventType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOne(EventType eventType)
        {
            _context.EventTypes.Update(eventType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOne(EventType eventType)
        {
            _context.Remove(eventType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
