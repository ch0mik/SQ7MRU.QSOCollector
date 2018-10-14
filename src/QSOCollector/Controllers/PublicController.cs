using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQ7MRU.Utils;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Mime;
using System.IO;
using System.Text;

namespace SQ7MRU.QSOCollector.Controllers
{
    [Route("api")]
    [AllowAnonymous]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly QSOColletorContext _context;
        private readonly ILogger _logger;

        public PublicController(QSOColletorContext context, ILogger<PublicController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// GET Stations
        /// </summary>
        /// <returns></returns>
        // GET: api/stations
        [HttpGet("stations")]
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
        [HttpGet("stations/{stationId}")]
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
        [HttpGet("stations/{stationId}/log")]
        public IEnumerable<Qso> GetLog(int stationId)
        {
            return _context.Log.Where(Q => Q.StationId == stationId)?.ToArray();
        }

        /// <summary>
        /// Get Station`s Logs
        /// </summary>
        /// <returns></returns>
        // GET: api/station/1/log
        [HttpGet("stations/{stationId}/export")]
        public ContentResult ExportLog(int stationId)
        {
            var station = _context.Station.Where(S => S.StationId == stationId).First<Station>();
            if (station != null)
            {
                return new ContentResult() {
                    Content = AdifHelper.ExportAsADIF(_context.Log.Where(Q => Q.StationId == stationId)?.ToArray<AdifRow>()),
                    ContentType = MediaTypeNames.Text.Plain,
                    StatusCode = 200 };
            }
            else
            {
                return new ContentResult() { StatusCode = 404 };
            }
            
        }

        /// <summary>
        /// Get Station`s Log Item
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="qsoId"></param>
        /// <returns></returns>
        // GET: api/station/1/log/1
        [HttpGet("stations/{stationId}/log/{qsoId}")]
        public IActionResult GetQso([FromRoute] int stationId, int qsoId)
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