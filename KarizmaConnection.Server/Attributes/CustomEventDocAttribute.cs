namespace KarizmaPlatform.Connection.Server.Attributes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public class CustomEventDocAttribute(
    string address,
    Type outputType,
    string? summary = null,
    string? description = null) : Attribute
{
    public string Address { get; } = address;
    public Type OutputType { get; } = outputType;
    public string? Summary { get; } = summary;
    public string? Description { get; } = description;
}