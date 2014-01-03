using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace ABPUtils
{
    class SimpleSerializer
    {
        /// <summary>
        /// Object to XML String
        /// </summary>
        public static string XmlSerialize<T>(T obj)
        {
            string xmlString;

            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }

            return xmlString;
        }

        /// <summary>
        /// XML String to Object
        /// </summary>
        public static T XmlDeserialize<T>(string xmlString)
        {
            T t;
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                using (var xmlReader = XmlReader.Create(xmlStream))
                {
                    var obj = xmlSerializer.Deserialize(xmlReader);
                    t = (T)obj;
                }
            }

            return t;
        }
    }
}
