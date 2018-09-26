using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ7MRU.QSOCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        private readonly QSOColletorContext _context;

        public StationsController(QSOColletorContext context)
        {
            _context = context;
        }

        // GET: api/Stations
        [HttpGet]
        public IEnumerable<Station> GetStation()
        {
            return _context.Station;
        }

        // GET: api/Stations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var station = await _context.Station.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        // PUT: api/Stations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStation([FromRoute] int id, [FromBody] Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != station.StationId)
            {
                return BadRequest();
            }

            _context.Entry(station).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Stations
        [HttpPost]
        public async Task<IActionResult> PostStation([FromBody] Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Station.Add(station);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStation", new { id = station.StationId }, station);
        }

        // DELETE: api/Stations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var station = await _context.Station.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }

            _context.Station.Remove(station);
            await _context.SaveChangesAsync();

            return Ok(station);
        }

        private bool StationExists(int id)
        {
            return _context.Station.Any(e => e.StationId == id);
        }
    }
}