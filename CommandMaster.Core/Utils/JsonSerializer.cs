using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CommandMaster.Core.Utils
{
    public static class JsonSerializer
    {
        public static string objectToString<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, obj);
                byte[] dataBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(dataBytes);
            }
        }

        public static T stringToObject<T>(string input)
        {
            T readConfig = (T)stringToObject(typeof(T), input);
            return readConfig;
        }

        public static object stringToObject(Type t, string input)
        {
            var serializer = new DataContractJsonSerializer(t);
            var mStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            return serializer.ReadObject(mStream);
        }

        public static string objectToFile<T>(T obj, string fileName)
        {
            var res = objectToString(obj);
            File.WriteAllText(fileName, res);
            return res;
        }

        public static T fileToObject<T>(string fileName)
        {
            return stringToObject<T>(File.ReadAllText(fileName));
        }

        public static object fileToObject(Type t, string fileName)
        {
            return stringToObject(t, File.ReadAllText(fileName));
        }
    }
}
