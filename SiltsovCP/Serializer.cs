using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiltsovCP
{
    internal static class Serializer
    {
        public static void SerializeXML<T>(ref T obj, string filename)
        {
            System.Xml.Serialization.XmlSerializer formatter = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(filename + ".xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, obj);

            }
        }

        public static T DeserializeXML<T>(string filename)
        {
            System.Xml.Serialization.XmlSerializer formatter = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(filename + ".xml", FileMode.OpenOrCreate))
            {
               return (T)formatter.Deserialize(fs);
            }

        }
    }
}
