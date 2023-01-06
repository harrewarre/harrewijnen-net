using System.Text.Json;

namespace Blog.JsonSupport;

public interface IJsonParser
{
    T ParseJson<T>(string json);
}

public class JsonParser : IJsonParser
{
    private readonly JsonSerializerOptions _metaSerializeOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public T ParseJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }
}