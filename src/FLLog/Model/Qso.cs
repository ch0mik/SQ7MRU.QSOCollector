using SQ7MRU.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQ7MRU.FLLog.Model
{
    public class Qso : AdifRow
    {
        public int QsoId { get; set; }
        public int StationId { get; set; }
    }
}
