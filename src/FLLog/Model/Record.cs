using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SQ7MRU.Utils;

namespace SQ7MRU.FLLog.Model
{
    public class Record
    {
        [Key]
        public Guid Guid { get; set; }
        public string SHA1 { get; set; }
        public string RawContent { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public bool Sent { get; set; } = false;
        public DateTime SentTime { get; set; }
        public int QsoId { get; set; }
    }
 }
