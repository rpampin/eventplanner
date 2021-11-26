﻿using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Data
{
    public class ConfigurationService
    {
        private readonly AppDBContext _context;

        public ConfigurationService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Models.Templates> GetTemplates()
            => await _context.Templates.SingleOrDefaultAsync() ?? new Models.Templates();

        public async Task SaveTemplates(Models.Templates templates)
        {
            if (templates.Id == Guid.Empty)
                await _context.Templates.AddAsync(templates);
            else
                _context.Templates.Update(templates);

            await _context.SaveChangesAsync();
        }
    }
}
