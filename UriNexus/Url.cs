namespace ToolBX.UriNexus;

public sealed record Url
{
    /// <summary>
    /// Ex : http
    /// </summary>
    public string Scheme
    {
        get => _scheme;
        init => _scheme = value?.TrimEnd('/', ':')?.Trim(' ') ?? string.Empty;
    }
    private readonly string _scheme = string.Empty;

    /// <summary>
    /// Ex : https://user:pass@www.something.com
    /// </summary>
    public UserInfo? UserInfo { get; init; }

    /// <summary>
    /// Ex : www.something.com
    /// </summary>
    public string Host
    {
        get => _host;
        init => _host = value?.Trim('/', ' ') ?? string.Empty;
    }
    private readonly string _host = string.Empty;

    public int? Port { get; init; }

    /// <summary>
    /// Ex : www.something.com/path#fragment
    /// </summary>
    public string Fragment
    {
        get => _fragment;
        init => _fragment = value?.Trim('#', ' ') ?? string.Empty;
    }
    private readonly string _fragment = string.Empty;

    /// <summary>
    /// Ex : https://www.something.com/path
    /// </summary>
    public IReadOnlyList<string> Path
    {
        get => _path;
        init => _path = value?.Select(x => x.Trim('/', ' ')).ToReadOnlyList() ?? ReadOnlyList<string>.Empty;
    }
    private readonly IReadOnlyList<string> _path = ReadOnlyList<string>.Empty;

    public UrlParameterList Parameters
    {
        get => _parameters;
        init => _parameters = value?.ToUrlParameterList() ?? new UrlParameterList();
    }
    private readonly UrlParameterList _parameters = new();

    public Url WithParameter(string name, object value) => WithParameters(new UrlParameter(name, value));

    public Url WithParameters(params UrlParameter[] parameters) => WithParameters((IEnumerable<UrlParameter>)parameters);

    public Url WithParameters(IEnumerable<UrlParameter> parameters)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        return this with
        {
            Parameters = Parameters.With(parameters)
        };
    }

    public Url AppendPath(params string[] path) => AppendPath(path as IEnumerable<string>);

    public Url AppendPath(IEnumerable<string> path)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));
        var pathList = path.Select(x => x?.Trim(' ', '/')).ToReadOnlyList();
        if (pathList.Any(string.IsNullOrWhiteSpace)) throw new ArgumentException(Exceptions.AddingEmptyPathSegment);

        if (!pathList.Any())
            return this;

        return this with
        {
            Path = Path.Concat(pathList).ToReadOnlyList()!
        };
    }

    public override string ToString()
    {
        var output = new StringBuilder();
        if (!string.IsNullOrWhiteSpace(Scheme))
            output.Append($"{Scheme}://");

        if (UserInfo != null)
            output.Append($"{UserInfo}@");

        if (!string.IsNullOrWhiteSpace(Host))
            output.Append($"{Host}{(Port.HasValue ? $":{Port}" : string.Empty)}/");

        if (Path.Any())
            output.Append(string.Join('/', Path));

        if (Parameters.Any())
            output.Append(output.Length == 0 ? Parameters : $"?{Parameters}");

        if (!string.IsNullOrWhiteSpace(Fragment))
            output.Append(output.Length == 0 ? Fragment : $"#{Fragment}");

        return output.ToString();
    }

    public static implicit operator string(Url url) => url?.ToString() ?? string.Empty;
}