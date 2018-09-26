using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQ7MRU.QSOCollector.Controllers;
using System.Linq;

namespace SQ7MRU.QSOCollector.Tests
{
    [TestClass]
    public class QSOCollectorTest
    {
        public QSOCollectorTest()
        {
            InitContext();
        }

        private QSOColletorContext _context;

        public void InitContext()
        {
            var builder = new DbContextOptionsBuilder<QSOColletorContext>().UseInMemoryDatabase("QSOCollector");

            var context = new QSOColletorContext(builder.Options, new LoggerFactory());
            var stations = Enumerable.Range(1, 3)
                .Select(i => new Station() { StationId = i, Callsign = "SQ7MRU", Locator = "JO91vr", Operator = "Pawel", QTH = "Koluszki", Name = $"Koluszki #{i}" });
            context.Station.AddRange(stations);
            int changed = context.SaveChanges();
            _context = context;
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetStationByIdAsync()
        {
            string expectedName = "Koluszki #2";
            var controller = new StationsController(_context);
            var result = await controller.GetStation(2) as OkObjectResult;
            if (result?.Value as Station != null)
            {
                Assert.AreEqual("Koluszki #2", ((Station)result.Value).Name);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}