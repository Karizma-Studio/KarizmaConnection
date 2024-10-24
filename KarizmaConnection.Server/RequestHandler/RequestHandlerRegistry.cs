namespace KarizmaConnection.Server.RequestHandler;

internal sealed class RequestHandlerRegistry
{
    private readonly Dictionary<string, RequestHandlerAction> handlerActions = new();
    internal List<RequestHandlerAction> GetAllHandlerActions => handlerActions.Values.ToList();

    internal void AddHandler(string address, RequestHandlerAction requestHandlerAction)
    {
        handlerActions.Add(address.ToLowerInvariant(), requestHandlerAction);
    }

    internal bool TryGetHandler(string address, out RequestHandlerAction requestHandlerAction)
    {
        return handlerActions.TryGetValue(address.ToLowerInvariant(), out requestHandlerAction!);
    }
}