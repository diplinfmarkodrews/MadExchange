using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MadXchange.Common.Helpers
{
    public static class Converter
    {
        //Convert Object to byteArray
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                formatter.Serialize(memStream, obj);
                return memStream.ToArray();
            }
        }
        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            if (arrBytes is null) return null;
            using (var memStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = formatter.Deserialize(memStream);
                return obj;
            }
        }
    }
}
