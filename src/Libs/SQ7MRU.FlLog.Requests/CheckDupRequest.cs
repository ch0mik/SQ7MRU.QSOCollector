
namespace SQ7MRU.FlLog.Requests
{
    public class CheckDupRequest
    {
        public string Call { get; set; }
        public string Mode { get; set; }
        public int TimeSpan { get; set; } = 1;
        public string Freq { get; set; }
        public string State { get; set; }
        public string XchgIn { get; set; } 
        
    }
}