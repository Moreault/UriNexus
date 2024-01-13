namespace ToolBX.UriNexus.Collections;

[JsonConverter(typeof(UrlParameterListJsonConverter))]
public sealed record UrlParameterList : IEnumerable<UrlParameter>, IEquatable<IEnumerable<UrlParameter>>
{
    private readonly ReadOnlyList<UrlParameter> _items;

    public int Count => _items.Count;

    public string this[string name] => _items.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCulture))?.Value?.ToString() ?? string.Empty;

    public UrlParameterList()
    {
        _items = ReadOnlyList<UrlParameter>.Empty;
    }

    public UrlParameterList(params UrlParameter[] parameters) : this(parameters as IEnumerable<UrlParameter>)
    {

    }

    public UrlParameterList(IEnumerable<UrlParameter> parameters)
    {
        _items = parameters?.ToReadOnlyList() ?? throw new ArgumentNullException(nameof(parameters));
    }

    public UrlParameterList With(string name, object value) => With(new UrlParameter(name, value));

    public UrlParameterList With(params UrlParameter[] parameters) => With(parameters as IEnumerable<UrlParameter>);

    public UrlParameterList With(IEnumerable<UrlParameter> parameters)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        var listParameters = parameters as IReadOnlyList<UrlParameter> ?? parameters.ToList();
        if (listParameters.Any(x => x == null!)) throw new ArgumentException(Exceptions.AddingNullParameters);

        var duplicateNames = _items.Where(x => listParameters.Any(y => x.Name == y.Name)).ToList();
        if (duplicateNames.Count == 1)
            throw new Exception(string.Format(Exceptions.AddingOneDuplicateParameter, duplicateNames.Single().Name));
        if (duplicateNames.Count > 1)
            throw new Exception(string.Format(Exceptions.AddingMultipleDuplicateParameters, string.Join(", ", duplicateNames.Select(x => x.Name))));

        return new UrlParameterList(_items.Concat(listParameters));
    }

    public UrlParameterList Without(string name, StringComparison comparison = StringComparison.InvariantCulture)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        var item = _items.SingleOrDefault(x => string.Equals(x.Name, name, comparison));
        if (item == null) throw new InvalidOperationException(string.Format(Exceptions.RemovingNonExistentParameter, name));
        return new UrlParameterList(_items.Where(x => !string.Equals(x.Name, name, comparison)));
    }

    public bool Equals(UrlParameterList? other) => Equals(other as IEnumerable<UrlParameter>);

    public bool Equals(IEnumerable<UrlParameter>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _items.SequenceEqual(other);
    }

    public static bool operator ==(UrlParameterList? a, IEnumerable<UrlParameter>? b) => a is null && b is null || a is not null && a.Equals(b);

    public static bool operator !=(UrlParameterList? a, IEnumerable<UrlParameter>? b) => !(a == b);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<UrlParameter> GetEnumerator() => _items.GetEnumerator();

    public override int GetHashCode() => _items.GetHashCode();

    public override string ToString() => string.Join('&', _items);
}