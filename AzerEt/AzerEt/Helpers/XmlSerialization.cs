using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

namespace AzerEt.Helper
{
    public class XmlSerialaizer
    {
        public T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }
    }

    [Serializable]
    public class TKKPG
    {
        public Response Response { get; set; }
    }
    [Serializable]
    public class Response
    {
        public string Operation { get; set; }
        public int Status { get; set; }
        public Order Order { get; set; }
    }
    [Serializable]
    public class Order
    {

        public int OrderID { get; set; }
        public string SessionID { get; set; }
        public string URL { get; set; }
        public string OrderStatus { get; set; }
    }
}