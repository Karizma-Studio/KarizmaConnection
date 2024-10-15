namespace KarizmaCon.Server.RequestHandler;

internal sealed class RequestHandlerRegistry
{
    private readonly Dictionary<string, RequestHandlerAction> handlers = new();

    internal void AddHandler(string address, RequestHandlerAction requestHandlerAction)
    {
        handlers.Add(address.ToLowerInvariant(), requestHandlerAction);
    }

    internal bool TryGetHandler(string address, out RequestHandlerAction requestHandlerAction)
    {
        return handlers.TryGetValue(address.ToLowerInvariant(), out requestHandlerAction!);
    }
}