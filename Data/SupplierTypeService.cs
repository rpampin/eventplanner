using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanner.Data
{
    public class SupplierTypeService
    {
        private readonly AppDBContext _context;

        public SupplierTypeService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplierType>> GetAll()
            => await _context.SupplierTypes
            .OrderBy(e => e.Name.ToLower())
            .ToListAsync();

        public async Task<SupplierType> GetOne(Guid id)
            => await _context.SupplierTypes.FirstOrDefaultAsync(t => t.Id.Equals(id));

        public async Task<bool> InsertOne(SupplierType eventType)
        {
            await _context.SupplierTypes.AddAsync(eventType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOne(SupplierType eventType)
        {
            _context.SupplierTypes.Update(eventType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOne(SupplierType eventType)
        {
            _context.Remove(eventType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
