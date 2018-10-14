using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ7MRU.QSOCollector.Services.EQSL
{
    public class Config
    {
        public string Callsign { set; get; }
        public string Password { get; set; }
        public string BaseImagesPath { get; set; }
        public int JobInterval { get; set; }
    }
}
