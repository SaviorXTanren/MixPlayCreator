using Newtonsoft.Json;

namespace MixPlayCreator.Base.Util
{
    public static class SerializerHelper
    {
        public static string SerializeObjectToString<T>(T data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        public static T DeserializeObjectFromString<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
