using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SQ7MRU.Utils;

namespace SQ7MRU.QSOCollector
{
    public class Qso : AdifRow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Key]
        public int QsoId { get; set; }
        
        [ForeignKey("FK_Log_QSO")]
        public int StationId { get; set; }
    }
}