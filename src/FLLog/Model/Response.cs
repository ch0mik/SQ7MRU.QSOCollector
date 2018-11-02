using System.Collections.Generic;
using System.Xml.Serialization;

namespace SQ7MRU.FLLog.Model
{
    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlElement(ElementName = "value")]
        public string[] Value { get; set; }
    }

    [XmlRoot(ElementName = "array")]
    public class Array
    {
        [XmlElement(ElementName = "data")]
        public Data Data { get; set; }
    }

    [XmlRoot(ElementName = "value")]
    public class Value
    {
        [XmlElement(ElementName = "array")]
        public Array Array { get; set; }
    }

    [XmlRoot(ElementName = "param")]
    public class Param
    {
        [XmlElement(ElementName = "value")]
        public virtual Value Value { get; set; }
    }

    [XmlRoot(ElementName = "params")]
    public class Params
    {
        [XmlElement(ElementName = "param")]
        public Param Param { get; set; }
    }


    [XmlRoot(ElementName = "methodResponse")]
    public class MethodResponse
    {
        [XmlElement(ElementName = "params")]
        public Params Params { get; set; }
    }
}