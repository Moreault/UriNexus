namespace ToolBX.UriNexus.Collections;

public static class UrlParameterListExtensions
{
    public static UrlParameterList ToUrlParameterList(this IEnumerable<UrlParameter> parameters) => new(parameters);
}