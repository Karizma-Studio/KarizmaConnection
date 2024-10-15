using System.Reflection;

namespace KarizmaCon.Server.RequestHandler;

internal sealed class RequestHandlerAction(Type handlerType, MethodInfo actionMethodInfo)
{
    public Type HandlerType { get; private set; } = handlerType;
    public MethodInfo ActionMethodInfo { get; private set; } = actionMethodInfo;
}