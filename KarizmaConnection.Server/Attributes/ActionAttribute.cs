namespace KarizmaConnection.Server.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ActionAttribute(string route, bool needAuthorizedUser = true) : Attribute
{
    public string Route { get; } = route;
    public bool NeedAuthorizedUser { get; } = needAuthorizedUser;
}