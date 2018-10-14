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
    public class EqslUploadService : BackgroundService
    {
        private readonly ILogger<EqslUploadService> _logger;
        private readonly IServiceProvider _provider;
        private readonly Config _config;

        public EqslUploadService(ILogger<EqslUploadService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _provider = serviceProvider;
            _config = configuration?.GetSection("EQSL.CC")?.Get<Config>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"{nameof(EqslUploadService)} is starting.");

            stoppingToken.Register(() =>
                    _logger.LogDebug($"{nameof(EqslUploadService)} background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"{nameof(EqslUploadService)} task doing background work.");

                using (IServiceScope scope = _provider.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<QSOColletorContext>())
                    {
                        foreach (Station station in context.Station)
                        {
                            try
                            {
                                var QsosToSend = context.Log.Where(Q => Q.StationId == station.StationId && Q.EQSL_QSL_SENT == "N").ToArray();
                                var adif = AdifHelper.ExportAsADIF(QsosToSend);
                                var c = new Uploader();
                                if (c.UploadAdif(adif, station.Callsign, station.HamID))
                                {
                                    //update QSOs
                                    foreach (Qso qso in QsosToSend)
                                    {
                                        qso.EQSL_QSL_SENT = "Y";
                                        qso.EQSL_QSLSDATE = DateTime.UtcNow.ToString("yyyyMMdd");
                                        context.SaveChanges();
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

            _logger.LogDebug($"{nameof(EqslUploadService)} background task is stopping.");
        }
    }
}