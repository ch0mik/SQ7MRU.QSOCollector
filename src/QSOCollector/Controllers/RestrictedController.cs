﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQ7MRU.FLLog.Requests;
using SQ7MRU.QSOCollector.Helpers;
using SQ7MRU.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ7MRU.QSOCollector.Controllers
{
    [Route("restricted")]
    [Authorize]
    [ApiController]
    public class RestrictedController : ControllerBase
    {
        private readonly QSOColletorContext _context;
        private readonly ILogger _logger;

        public RestrictedController(QSOColletorContext context, ILogger<RestrictedController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Station

        /// <summary>
        /// Insert Station Item
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        // POST: restricted/stations
        [HttpPost("stations")]
        public async Task<IActionResult> PostStation([FromBody] Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (StationExists(station.StationId))
            {

                return RedirectToAction("GetStation", "Public", new { station.StationId });
            }
            else
            {
                _context.Station.Add(station);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetStation", "Public", new { id = station.StationId }, station);
            }
        }

        /// <summary>
        /// Update Station Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        // PUT: restricted/stations/1
        [HttpPut("stations/{stationId}")]
        public async Task<IActionResult> PutStation([FromRoute] int stationId, [FromBody] Station station)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (stationId != station.StationId)
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
                if (!StationExists(stationId))
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

        /// <summary>
        /// Delete Station Item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: restricted/stations/1
        [HttpDelete("stations/{stationId}")]
        public async Task<IActionResult> DeleteStation([FromRoute] int stationId)
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

            _context.Station.Remove(station);
            await _context.SaveChangesAsync();

            return Ok(station);
        }

        /// <summary>
        /// Force Delete Station Item
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: restricted/stations/1/force
        [HttpDelete("stations/{stationId}/force")]
        public async Task<IActionResult> ForceDeleteStation([FromRoute] int stationId)
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

            station.Log = null;
            _context.Station.Remove(station);
            await _context.SaveChangesAsync();

            return Ok(station);
        }

        private bool StationExists(int stationId)
        {
            return _context.Station.Any(e => e.StationId == stationId);
        }

        private bool StationExists(Station station)
        {
            return _context.Station.Any(e => e == station);
        }

        #endregion Station

        #region Log

        /// <summary>
        /// Insert Station's Log Item
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="qso"></param>
        /// <returns></returns>
        // POST: restricted/stations/1/insert/qso
        [HttpPost("stations/{stationId}/insert/qso")]
        public async Task<IActionResult> PostQso([FromRoute] int stationId, [FromBody] Qso qso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!StationExists(stationId))
            {
                return NotFound();
            }
            else
            {
                qso.StationId = stationId;
                _context.Log.Add(qso);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetQso", "Public", new { id = qso.QsoId }, qso);
            }
        }

        /// <summary>
        /// Insert Station's Log Item from Adif Record and check duplicates
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="adifRow"></param>
        /// <param name="minutesAccept"></param>
        /// <returns></returns>
        // POST: restricted/stations/1/insert/adif/10
        [HttpPost("stations/{stationId}/insert/adif/{minutesAccept}")]
        public async Task<IActionResult> PostQso([FromRoute] int stationId, [FromBody] AdifRow adifRow, [FromRoute] int minutesAccept = 10)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!StationExists(stationId))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    Station station = _context.Station.Find(stationId);
                    Qso[] duplicates = _context.SearchDuplicates(station, adifRow, minutesAccept) ?? new Qso[0];
                    if (duplicates.Length > 0)
                    {
                        var duplicateQso = duplicates.First();
                        return RedirectToAction("GetQso", "Public", new {duplicateQso.StationId,  duplicateQso.QsoId });
                    }
                    else
                    {
                        Qso qso = Converters.Convert(adifRow, station);
                        _context.Log.Add(qso);
                        await _context.SaveChangesAsync();
                        return CreatedAtRoute(new { id = qso.QsoId }, qso);
                    }
                }
                catch(Exception exc)
                {
                    _logger.LogError(exc.Message, null);
                    return BadRequest(exc.Message);
                }
            }
        }

        /// <summary>
        /// Update Station's Log Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qso"></param>
        /// <returns></returns>
        // PUT restricted/stations/1/log/1
        [HttpPut("stations/{stationId}/log/{qsoId}")]
        public async Task<IActionResult> PutQso([FromRoute] int stationId, [FromRoute] int qsoId, [FromBody] Qso qso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (qsoId != qso.QsoId)
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
                if (!QsoExists(qsoId))
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

        // DELETE: api/Logs/5
        [HttpDelete("/stations/{stationId}/log/{qsoId}")]
        public async Task<IActionResult> DeleteQso([FromRoute] int stationId, int qsoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qso = await _context.Log.FindAsync(qsoId);
            if (qso == null)
            {
                return NotFound();
            }

            _context.Log.Remove(qso);
            await _context.SaveChangesAsync();

            return Ok(qso);
        }

        private bool QsoExists(int qsoId)
        {
            return _context.Log.Any(e => e.QsoId == qsoId);
        }

        #endregion Log

        #region FlLog


        /// <summary>
        /// Check duplicates
        /// </summary>
        /// <returns></returns>
        // POST: restricted/stations/1/check_dup
        [HttpPost("stations/{stationId}/check_dup")]
        public bool CheckDuplicates([FromRoute] int stationId, [FromBody] CheckDupRequest checkDupRequest)
        {
            Station station = _context.Station.Find(stationId);
            Qso[] duplicates = _context.SearchDuplicates(station, Converters.CheckDupToAdif(checkDupRequest), checkDupRequest?.TimeSpan ?? 1 ) ?? new Qso[0];
            return (duplicates.Length > 0);
        }

        /// <summary>
        /// Request record if it exists else return "NO_RECORD"
        /// </summary>
        /// <returns>Returns ADIF record</returns>
        // POST: restricted/stations/1/get_record
        [HttpPost("stations/{stationId}/get_record")]
        public AdifRow GetRecord([FromRoute] int stationId, [FromBody] string callSign)
        {
            return (from l in _context.Log
                    where l.CALL == callSign
                    select l)?.OrderByDescending(x => x.QSO_DATE)?.FirstOrDefault();
        }

        #endregion

    }
}