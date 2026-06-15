namespace ToolBX.UriNexus.Json;

public sealed class UrlParameterListJsonConverter : JsonConverter<UrlParameterList>
{
    private static readonly UrlParameterJsonConverter ParameterConverter = new();

    public override UrlParameterList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected StartArray token");
        }

        var parameters = new List<UrlParameter>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                break;
            }

            parameters.Add(ParameterConverter.Read(ref reader, typeof(UrlParameter), options));
        }

        return new UrlParameterList(parameters);
    }

    public override void Write(Utf8JsonWriter writer, UrlParameterList value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            ParameterConverter.Write(writer, item, options);
        }
        writer.WriteEndArray();
    }
}
