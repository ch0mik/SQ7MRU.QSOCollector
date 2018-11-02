using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ7MRU.FLLog
{
    public class Config
    {
        public string BaseUrl { get; set; }
        public int StationId { get; set; }
        public string JwtSecretKey { get; set; }
    }
}
