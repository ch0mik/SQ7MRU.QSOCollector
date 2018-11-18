using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQ7MRU.FLLog.Controllers;
using SQ7MRU.FLLog.Model;
using System;
using System.Linq;

namespace SQ7MRU.FLLog
{
    public class LocalBufferContext : DbContext
    {
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;

        public DbSet<Record> Records { get; set; }

        public LocalBufferContext(DbContextOptions<LocalBufferContext> options, ILoggerFactory loggerFactory = null) : base(options)
        {
            if (loggerFactory == null)
            {
                _loggerFactory = new LoggerFactory();
            }

            _logger = loggerFactory.CreateLogger(nameof(LocalBufferContext));
            _logger.LogInformation($"Initialize {nameof(LocalBufferContext)}");
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public Guid InsertRecord(string record)
        {
            string sha1 = XmlHelper.GetSHA1(record);
            if (Records.Where(B => B.SHA1 == sha1)?.Count() > 0)
            {
                return Records.Where(B => B.SHA1 == sha1).First().Guid;
            }
            else
            {
                Record buffer = new Record() { RawContent = record, SHA1 = sha1, CreateTime = DateTime.UtcNow };
                Records.Add(buffer);
                this.SaveChanges();
                return buffer.Guid;
            }
        }
    }
}
