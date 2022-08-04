namespace ToolBX.UriNexus;

public record UserInfo(string Name, string Password)
{
    public override string ToString() => $"{Name}:{Password}";
}