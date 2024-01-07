namespace ToolBX.UriNexus.Json;

public sealed class UrlParameterListJsonConverter : JsonConverter<UrlParameterList>
{
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

            var parameter = JsonSerializer.Deserialize<UrlParameter>(ref reader, options);
            if (parameter != null)
            {
                parameters.Add(parameter);
            }
        }

        return new UrlParameterList(parameters);
    }

    public override void Write(Utf8JsonWriter writer, UrlParameterList value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }
        writer.WriteEndArray();
    }
}
