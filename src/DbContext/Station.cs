using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQ7MRU.QSOCollector
{
    public class Station
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int StationId { get; set; }
        public string Name { get; set; }
        public string Callsign { get; set; }
        public string QTH { get; set; }
        public string Locator { get; set; }
        public string Operator { get; set; }
        public virtual  List<Qso> Log { get; set; }

    }
}