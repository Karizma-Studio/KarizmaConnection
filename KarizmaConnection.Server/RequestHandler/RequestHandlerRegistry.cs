namespace KarizmaPlatform.Connection.Server.RequestHandler;

internal static class RequestHandlerRegistry
{
    private static readonly Dictionary<string, RequestHandlerAction> HandlerActions = new();
    internal static List<RequestHandlerAction> GetAllHandlerActions => HandlerActions.Values.ToList();

    internal static void AddHandler(string address, RequestHandlerAction requestHandlerAction)
    {
        HandlerActions.Add(address.ToLowerInvariant(), requestHandlerAction);
    }

    internal static bool TryGetHandler(string address, out RequestHandlerAction requestHandlerAction)
    {
        return HandlerActions.TryGetValue(address.ToLowerInvariant(), out requestHandlerAction!);
    }
}