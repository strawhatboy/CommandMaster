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
            var serializer = new DataContractJsonSerializer(typeof(T));
            var mStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            T readConfig = (T)serializer.ReadObject(mStream);
            return readConfig;
        }
    }
}
