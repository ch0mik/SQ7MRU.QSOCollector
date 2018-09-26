using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ7MRU.QSOCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly QSOColletorContext _context;

        public LogsController(QSOColletorContext context)
        {
            _context = context;
        }

        // GET: api/Logs
        [HttpGet]
        public IEnumerable<Qso> GetLog()
        {
            return _context.Log;
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQso([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qso = await _context.Log.FindAsync(id);

            if (qso == null)
            {
                return NotFound();
            }

            return Ok(qso);
        }

        // PUT: api/Logs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQso([FromRoute] int id, [FromBody] Qso qso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qso.QsoId)
            {
                return BadRequest();
            }

            _context.Entry(qso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QsoExists(id))
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

        // POST: api/Logs
        [HttpPost]
        public async Task<IActionResult> PostQso([FromBody] Qso qso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Log.Add(qso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQso", new { id = qso.QsoId }, qso);
        }

        // DELETE: api/Logs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQso([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qso = await _context.Log.FindAsync(id);
            if (qso == null)
            {
                return NotFound();
            }

            _context.Log.Remove(qso);
            await _context.SaveChangesAsync();

            return Ok(qso);
        }

        private bool QsoExists(int id)
        {
            return _context.Log.Any(e => e.QsoId == id);
        }
    }
}