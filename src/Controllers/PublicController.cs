using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ7MRU.QSOCollector.Controllers
{
    [Route("api")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly QSOColletorContext _context;

        public PublicController(QSOColletorContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET Stations
        /// </summary>
        /// <returns></returns>
        // GET: api/stations
        [HttpGet("/stations")]
        public IEnumerable<Station> GetStation()
        {
            return _context.Station;
        }

        /// <summary>
        /// Get Station Info
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        // GET: api/stations/1
        [HttpGet("/stations/{stationId}")]
        public async Task<IActionResult> GetStation([FromRoute] int stationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var station = await _context.Station.FindAsync(stationId);

            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
        }

        /// <summary>
        /// Get Station`s Logs
        /// </summary>
        /// <returns></returns>
        // GET: api/station/1/log
        [HttpGet("/stations/{stationId}/log")]
        public IEnumerable<Qso> GetLog()
        {
            return _context.Log;
        }

        /// <summary>
        /// Get Station`s Log Item
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="qsoId"></param>
        /// <returns></returns>
        // GET: api/station/1/log/1
        [HttpGet("/stations/{stationId}/log/{qsoId}")]
        public async Task<IActionResult> GetQso([FromRoute] int stationId, int qsoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qso = _context.Log.Where(Q => Q.StationId == stationId && Q.QsoId == qsoId).First();

            if (qso == null)
            {
                return NotFound();
            }

            return Ok(qso);
        }
    }
}