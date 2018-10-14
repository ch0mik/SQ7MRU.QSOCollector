namespace SQ7MRU.FLLog.Model
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("methodCall", IsNullable = false)]
    public class MethodCall
    {
        public string methodName
        {
            get; set;
        }

        [System.Xml.Serialization.XmlArrayItemAttribute("param", IsNullable = false)]
        public methodCallParam[] @params
        {
            get; set;
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class methodCallParam
    {
        public string value
        {
            get; set;
        }
    }
}