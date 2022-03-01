using EventPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventPlanner.Data
{
    public class PackageService
    {
        private readonly AppDBContext _context;

        public PackageService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Package>> GetAll()
            => await _context.Packages
            .OrderBy(e => e.Name.ToLower())
            .ToListAsync();

        public async Task<Package> GetOne(Guid id)
            => await _context.Packages.FirstOrDefaultAsync(t => t.Id.Equals(id));

        public async Task<bool> InsertOne(Package eventType)
        {
            await _context.Packages.AddAsync(eventType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOne(Package eventType)
        {
            _context.Packages.Update(eventType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOne(Package eventType)
        {
            _context.Remove(eventType);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
