using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SQ7MRU.Utils;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SQ7MRU.QSOCollector.Services.EQSL
{
    public class EqslDownloadService : BackgroundService
    {
        private readonly ILogger<EqslDownloadService> _logger;
        private readonly IServiceProvider _provider;
        private readonly Config _config;

        public EqslDownloadService(ILogger<EqslDownloadService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _provider = serviceProvider;
            _config = configuration?.GetSection("EQSL.CC")?.Get<Config>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"{nameof(EqslDownloadService)} is starting.");

            stoppingToken.Register(() =>
                    _logger.LogDebug($"{nameof(EqslDownloadService)} background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"{nameof(EqslDownloadService)} task doing background work.");

                using (IServiceScope scope = _provider.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<QSOColletorContext>())
                    {
                        foreach (Station station in context.Station)
                        {
                            try
                            {
                                var d = new Downloader(station.Callsign, _config.Password);
                                var adif = d.GetSingleAdif(station.Callsign, _config.Password, station.HamID);
                                using (AdifReader ar = new AdifReader(adif))
                                {
                                    foreach (AdifRow r in ar.GetAdifRows())
                                    {
                                        var founded = context.SearchDuplicates(station, r, 10).Where(Q => (Q.EQSL_QSL_RCVD != "Y" || string.IsNullOrEmpty(Q.EQSL_QSL_RCVD))).ToArray();

                                        //update QSOs
                                        foreach (Qso qso in founded)
                                        {
                                            qso.EQSL_QSL_RCVD = "Y";
                                            qso.EQSL_QSLRDATE = DateTime.UtcNow.ToString("yyyyMMdd");
                                            if(string.IsNullOrEmpty(qso.SUBMODE) && !string.IsNullOrEmpty(r.SUBMODE))
                                            {
                                                qso.SUBMODE = r.SUBMODE;
                                            }
                                            if (string.IsNullOrEmpty(qso.GRIDSQUARE) && !string.IsNullOrEmpty(r.GRIDSQUARE))
                                            {
                                                qso.GRIDSQUARE = r.GRIDSQUARE;
                                            }
                                            context.SaveChanges();
                                            _logger.LogInformation($"{qso.CALL},{qso.QSO_DATE},{qso.MODE},{qso.BAND} has been updated from eQSL.cc");
                                        }
                                    }
                                }
                            }
                            catch (Exception exc)
                            {
                                _logger.LogError(exc.Message);
                            }
                        }
                    }
                }
                await Task.Delay(new TimeSpan(_config.JobInterval, 0, 0));
            }

            _logger.LogDebug($"{nameof(EqslDownloadService)} background task is stopping.");
        }
    }
}