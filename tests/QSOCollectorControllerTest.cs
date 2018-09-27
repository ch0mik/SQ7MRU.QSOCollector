using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQ7MRU.QSOCollector.Controllers;
using SQ7MRU.QSOCollector.Helpers;
using SQ7MRU.Utils;
using System.IO;
using System.Linq;

namespace SQ7MRU.QSOCollector.Tests
{
    [TestClass]
    public class QSOCollectorTest
    {
        private static QSOColletorContext _context;

        #region Init

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext testContext)
        {
            var builder = new DbContextOptionsBuilder<QSOColletorContext>().UseInMemoryDatabase("QSOCollector");

            var context = new QSOColletorContext(builder.Options, new LoggerFactory());

            var stations = Enumerable.Range(1, 3)
                .Select(i => new Station() { StationId = i, Callsign = "SQ7MRU", Locator = "JO91vr", Operator = "Pawel", QTH = "Koluszki", Name = $"Koluszki #{i}" });
            context.Station.AddRange(stations);
            context.SaveChanges();

            string adif = File.ReadAllText("Sample.ADIF");
            using (AdifReader ar = new AdifReader(adif))
            {
                foreach (Station s in context.Station)
                {
                    foreach (AdifRow row in ar.GetAdifRows())
                    {
                        context.Log.Add(Converters.Convert(row, s));
                        context.SaveChanges();
                    }
                }
            }

            _context = context;
        }

        #endregion Init

        [TestMethod]
        public void TestGetAllStations()
        {
            var controller = new PublicController(_context);
            var result = controller.GetStation();
            Assert.IsTrue(result.Count() == 3);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task TestGetStationByIdAsync()
        {
            string expectedName = "Koluszki #2";
            var controller = new PublicController(_context);
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