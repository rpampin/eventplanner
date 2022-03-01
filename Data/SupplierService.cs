using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanner.Data
{
    public class SupplierService
    {
        private readonly AppDBContext _context;

        public SupplierService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAll(Guid eventId)
            => await _context.Suppliers
            .Include(e => e.Type)
            .Where(e => e.Event.Id.Equals(eventId))
            .OrderBy(e => e.Name.ToLower())
            .ToListAsync();

        public async Task<Supplier> GetOne(Guid id)
            => await _context.Suppliers
                .Include(e => e.Type)
                .Include(e => e.Attachments)
                .FirstOrDefaultAsync(t => t.Id.Equals(id));

        public async Task<bool> InsertOne(Supplier Supplier)
        {
            await _context.Suppliers.AddAsync(Supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOne(Supplier Supplier)
        {
            _context.Suppliers.Update(Supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOne(Supplier Supplier)
        {
            _context.Remove(Supplier);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
