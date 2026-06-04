using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;

namespace EventEase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HealthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() => Ok(new { status = "alive", time = System.DateTime.UtcNow });

        [HttpGet("db")]
        public async Task<IActionResult> CheckDb()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (!canConnect) return Ok(new { database = "cannot connect" });

                var venueCount = await _context.Venues.CountAsync();
                return Ok(new { database = "connected", venueCount = venueCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
