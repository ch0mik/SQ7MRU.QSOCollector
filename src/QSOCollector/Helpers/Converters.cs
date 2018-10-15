using Newtonsoft.Json;
using SQ7MRU.FlLog.Requests;
using SQ7MRU.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SQ7MRU.QSOCollector.Helpers
{
    public static class Converters
    {
        private static CultureInfo _provider = CultureInfo.InvariantCulture;

        public static Qso Convert(AdifRow adifRow, Station station = null)
        {
            var obj = JsonConvert.DeserializeObject<Qso>(JsonConvert.SerializeObject(adifRow));
            if (station != null)
            {
                obj.StationId = station.StationId;
            }
            return obj;
        }

        public static Qso[] Convert(AdifRow[] rows, Station station = null)
        {
            List<Qso> result = new List<Qso>();
            foreach (AdifRow adif in rows)
            {
                result.Add(Convert(adif, station));
            }
            return result?.ToArray();
        }

        public static Qso[] ReadAdif(string adifContent, Station station = null)
        {
            using (var reader = new AdifReader(adifContent))
            {
                return Convert(reader.GetAdifRows()?.ToArray(), station);
            }
        }

        public static DateTime GetDateTime(string qso_date, string time_on)
        {
            //http://www.adif.org/306/ADIF_306.htm#Date + http://www.adif.org/306/ADIF_306.htm#Time
            return DateTime.ParseExact($"{qso_date} {time_on?.Substring(0, 4)}", "yyyyMMdd HHmm", _provider);
        }

        public static AdifRow CheckDupToAdif(CheckDupRequest checkDupRequest)
        {
            AdifRow adif = new AdifRow()
            {
                CALL = checkDupRequest.Call,
                MODE = checkDupRequest.Mode,
                QSO_DATE = DateTime.UtcNow.ToString("yyyyMMdd"),
                TIME_ON = DateTime.UtcNow.ToString("HHmm")
            };

            if(!string.IsNullOrEmpty(checkDupRequest.Freq) && checkDupRequest?.Freq != "0")
            {
                double freq;
                if (double.TryParse(checkDupRequest.Freq, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out freq))
                {
                    freq = freq / 1000;
                    adif.FREQ = freq.ToString("F6",CultureInfo.InvariantCulture);
                    adif.BAND = AdifHelper.FreqToBand(freq);
                }
            }

            if (!string.IsNullOrEmpty(checkDupRequest.State) && checkDupRequest?.State!="0")
            {
                adif.STATE = checkDupRequest.State;
            }

            return adif;
        }


       
    }
}