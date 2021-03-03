using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlanner.Data;
using EventPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Configuration>> Get()
        {
            return await _context.Configurations.FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Configuration>> Post(Configuration configuration)
        {
            Guid confId = await _context.Configurations.Select(c => c.Id).FirstOrDefaultAsync();
            if (confId != Guid.Empty)
            {
                configuration.Id = confId;
                _context.Entry(configuration).State = EntityState.Modified;
            }
            else
            {
                await _context.Configurations.AddAsync(configuration);
            }

            await _context.SaveChangesAsync();

            return configuration;
        }


        [HttpGet("smtp")]
        public async Task<ActionResult<SmtpConfig>> GetStmp()
        {
            return await _context.SmtpConfig.FirstOrDefaultAsync();
        }

        [HttpPost("smtp")]
        public async Task<ActionResult<SmtpConfig>> PostSmtp(SmtpConfig config)
        {
            if (config.Id == Guid.Empty)
            {
                var existingConfig = await _context.SmtpConfig.FirstOrDefaultAsync();
                if (existingConfig == null)
                {
                    await _context.SmtpConfig.AddAsync(config);
                }
                else
                {
                    config.Id = existingConfig.Id;
                    _context.Entry(config).State = EntityState.Modified;
                }
            }
            else
            {
                _context.Entry(config).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return config;
        }
    }
}
