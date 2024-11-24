namespace KarizmaPlatform.Connection.Server.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RequestHandlerAttribute(string route) : Attribute
{
    public string Route { get; } = route;
}