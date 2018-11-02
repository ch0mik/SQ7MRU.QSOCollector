using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQ7MRU.QSOCollector.Helpers;
using SQ7MRU.Utils;
using System;
using System.Linq;

namespace SQ7MRU.QSOCollector
{
    public class QSOColletorContext : DbContext
    {
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;

        public DbSet<Station> Station { get; set; }
        public DbSet<Qso> Log { get; set; }
               
        public QSOColletorContext(DbContextOptions<QSOColletorContext> options, ILoggerFactory loggerFactory = null) : base(options)
        {
            if (loggerFactory == null)
            {
                _loggerFactory = new LoggerFactory();
            }

            _logger = loggerFactory.CreateLogger(nameof(QSOColletorContext));
            _logger.LogInformation($"Initialize {nameof(QSOColletorContext)}");
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        #region Helpers

        public Qso[] SearchDuplicates(Station station, AdifRow adif, int minutesAccept)
        {
            return this.Log.Where(Q => Q.StationId == station.StationId
                        && Q.CALL.ToUpper() == adif.CALL.ToUpper()
                        && Q.BAND.ToLower() == adif.BAND.ToLower()
                        && Math.Abs((Converters.GetDateTime(Q.QSO_DATE, Q.TIME_ON) - Converters.GetDateTime(adif.QSO_DATE, adif.TIME_ON)).TotalMinutes) < minutesAccept
                        )?.ToArray();
        }

        public Qso[] SearchDuplicates(AdifRow adif, int minutesAccept)
        {
            return this.Log.Where(Q => Q.CALL.ToUpper() == adif.CALL.ToUpper()
                        && Q.BAND.ToLower() == adif.BAND.ToLower()
                        && Math.Abs((Converters.GetDateTime(Q.QSO_DATE, Q.TIME_ON) - Converters.GetDateTime(adif.QSO_DATE, adif.TIME_ON)).TotalMinutes) < minutesAccept
                        )?.ToArray();
        }

        public int InsertAdif(Station station, Qso row, int minutesAccept = 10)
        {
            int result = -1;

            row.StationId = station.StationId;

            if (SearchDuplicates(station, row, minutesAccept)?.Length == 0)
            {
                this.Log.Add(row);
                if (this.SaveChanges() > 0)
                {
                    _logger.LogInformation($"Insert QSO {AdifHelper.ConvertToString(row)} in Log {station.StationId}");
                    result = row.QsoId;
                }
            }
            else
            {
                _logger.LogWarning($"Duplicate for QSO {AdifHelper.ConvertToString(row)} in Log {station.StationId}");
            }

            return result;
        }
        
        #endregion
    }
}
