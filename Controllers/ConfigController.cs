using System;
using System.Collections;
using System.Collections.Generic;
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

        public class Config
        {
            public SmtpConfig SmtpConfig { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<Config>> Get()
        {
            var rv = new Config();

            rv.SmtpConfig = await _context.SmtpConfig.FirstOrDefaultAsync();

            return rv;
        }

        [HttpPost("smtp")]
        public async Task<ActionResult<SmtpConfig>> PostSmtpConfig(SmtpConfig config)
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
