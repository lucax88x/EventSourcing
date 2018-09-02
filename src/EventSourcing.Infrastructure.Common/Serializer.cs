using System;
using Newtonsoft.Json;

namespace EventSourcing.Infrastructure.Common
{
    public class Serializer
    {
        public string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        public object Deserialize(Type type, string json)
        {
            return JsonConvert.DeserializeObject(json, type);
        }
    }
}