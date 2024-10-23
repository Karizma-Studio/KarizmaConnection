using System.Reflection;

namespace KarizmaConnection.Server.RequestHandler;

internal sealed class RequestHandlerAction(
    Type handlerType,
    MethodInfo actionMethodInfo,
    bool needAuthorizedUser)
{
    public Type HandlerType { get; private set; } = handlerType;
    public MethodInfo ActionMethodInfo { get; private set; } = actionMethodInfo;
    public bool NeedAuthorizedUser { get; } = needAuthorizedUser;
}