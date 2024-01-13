namespace ToolBX.UriNexus.Json;

public sealed class UrlParameterJsonConverter : JsonConverter<UrlParameter>
{
    public override UrlParameter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var name = string.Empty;
        var value = string.Empty;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new UrlParameter(name, value);
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    case "Name":
                        name = reader.GetString();
                        break;
                    case "Value":
                        value = reader.GetString();
                        break;
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, UrlParameter value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Name", value.Name);
        writer.WriteString("Value", value.Value?.ToString() ?? string.Empty);
        writer.WriteEndObject();
    }
}
