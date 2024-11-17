using GameFramework;
using Newtonsoft.Json;

namespace Game.Gameplay;

public class NewtonJsonHelper:Utility.Json.IJsonHelper
{
    public string ToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public T ToObject<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    public object ToObject(Type objectType, string json)
    {
        return JsonConvert.DeserializeObject(json, objectType);
    }
}