using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SQ7MRU.FLLog.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SQ7MRU.FLLog.Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _provider;
        private QsoCollectorClient client;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _provider = serviceProvider;
            client = new QsoCollectorClient(configuration);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"{nameof(Worker)} is starting.");

            stoppingToken.Register(() =>
                    _logger.LogDebug($"{nameof(Worker)} background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"{nameof(Worker)} task doing background work.");

                using (IServiceScope scope = _provider.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<LocalBufferContext>())
                    {
                        foreach (var buffer in context.Records.Where(R => R.Sent == false))
                        {
                            try
                            {
                                int QsoId =  client.AddRecord(XmlHelper.ConvertToAdifRow(buffer.RawContent));

                                if (QsoId > 0)
                                {
                                    buffer.Sent = true;
                                    buffer.SentTime = DateTime.UtcNow;
                                    buffer.QsoId = QsoId;
                                    context.Records.Update(buffer);
                                    context.SaveChanges();
                                    _logger.LogInformation($"record {buffer.SHA1} sent; QsoId : {QsoId}");
                                }
                                else
                                {
                                    _logger.LogError($"record {buffer.SHA1} not sent");
                                }
                            }
                            catch (Exception exc)
                            {
                                _logger.LogError(exc.Message);
                            }
                        }
                    }
                }
                await Task.Delay(new TimeSpan(0,0,10));
            }

            _logger.LogDebug($"{nameof(Worker)} background task is stopping.");
        }
    }
}
