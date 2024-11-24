using System.Reflection;

namespace KarizmaPlatform.Connection.Server.RequestHandler;

internal sealed class RequestHandlerAction(
    string address,
    Type handlerType,
    MethodInfo actionMethodInfo,
    bool needAuthorizedUser)
{
    public string Address { get; private set; } = address;
    public Type HandlerType { get; private set; } = handlerType;
    public MethodInfo ActionMethodInfo { get; private set; } = actionMethodInfo;
    public bool NeedAuthorizedUser { get; } = needAuthorizedUser;
}