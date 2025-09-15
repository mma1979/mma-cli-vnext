namespace MmaSolution.EntityFramework.Infrastrcture.ValueConverters;

public class JsonDictionaryConverter : ValueConverter<Dictionary<string, object>, string>
{
    public JsonDictionaryConverter() : base(
        dict => JsonConvert.SerializeObject(dict),
        str => DeserializeToObjectDictionary(str)
    )
    {
    }

    private static Dictionary<string, object> DeserializeToObjectDictionary(string json)
    {
        if (string.IsNullOrEmpty(json))
            return [];

        var jObject = JObject.Parse(json);
        return jObject.ToObject<Dictionary<string, object>>();
    }
}
