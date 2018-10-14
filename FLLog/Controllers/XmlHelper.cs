using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SQ7MRU.FLLog.Controllers
{
    public static class XmlHelper
    {
        public static T XmlDeserializeFromString<T>(string objectData)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(objectData))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

    }
}
