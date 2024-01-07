namespace ToolBX.UriNexus;

[JsonConverter(typeof(UrlParameterJsonConverter))]
public sealed record UrlParameter
{
    //TODO 3.0.0 required
    public string Name
    {
        get => _name;
        init
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(Name));
            _name = value;
        }
    }
    private readonly string _name = string.Empty;

    //TODO 3.0.0 required
    public object Value
    {
        get => _value;
        init
        {
            if (value == null || value is string valueAsString && string.IsNullOrWhiteSpace(valueAsString)) throw new ArgumentNullException(nameof(Value));
            _value = value.ToString()!;
        }
    }
    private readonly string _value = string.Empty;

    public UrlParameter(string name, object value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString() => $"{Name}={Value}";
}