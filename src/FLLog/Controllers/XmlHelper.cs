using Newtonsoft.Json;
using SQ7MRU.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SQ7MRU.FLLog.Controllers
{
    public static class XmlHelper
    {
        public static T DeserializeFromString<T>(string objectData)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(objectData))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);

                return textWriter.ToString();
            }
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.Default; } }
        }

        public static string ConvertToString(AdifRow row)
        {
            StringBuilder sbRow = new StringBuilder();
            foreach (var pi in row.GetType().GetRuntimeProperties())
            {
                var v = pi.GetValue(row, null) as string;
                if (!string.IsNullOrEmpty(v))
                {
                    sbRow.Append(MakeTagValue(pi.Name, v));
                }
            }

            return $"{sbRow.ToString()}<EOR>";
        }

        private static string MakeTagValue(string tag, string value)
        {
            return $"<{tag?.ToUpper()}:{value?.Length}>{value} ";
        }

        public static string ConvertToAdifRow(string record)
        {
            AdifRow AdifRow = new AdifRow();

            record = record.Replace("<eor>", "");
            string[] x = Regex.Split(record.Replace("\n", "").Replace("\r", ""), @"<([^:]+):\d+[^>]*>").ToArray();
            List<string> l = new List<string>(x);
            l.RemoveAt(0);
            x = l.ToArray();

            var dic = new Dictionary<string, string>();
            if (x.Length % 2 == 0)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    if (!string.IsNullOrEmpty(x[i + 1]))
                    {
                        dic.Add(x[i].ToUpper(), x[i + 1]);
                    }
                    i++;
                }

                if (dic.ContainsKey("BAND") && dic.ContainsKey("FREQ"))
                {
                    dic["BAND"] = AdifHelper.FreqToBand(double.Parse(dic["FREQ"], CultureInfo.InvariantCulture));
                }
                else if (dic.ContainsKey("FREQ"))
                {
                    dic.Add("BAND", AdifHelper.FreqToBand(double.Parse(dic["FREQ"], CultureInfo.InvariantCulture)));
                }
            }

            return JsonConvert.SerializeObject(dic);
        }
    }
}