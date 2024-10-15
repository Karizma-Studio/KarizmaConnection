namespace KarizmaCon.Server.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RequestHandlerAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}