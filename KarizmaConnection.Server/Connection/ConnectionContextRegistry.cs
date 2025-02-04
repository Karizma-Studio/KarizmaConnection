using KarizmaPlatform.Connection.Server.Interfaces;

namespace KarizmaPlatform.Connection.Server.Connection;

internal static class ConnectionContextRegistry
{
    private static readonly Dictionary<string, IConnectionContext> ConnectionIdMap = new();
    private static readonly Dictionary<object, string> AuthorizationIdMap = new();

    internal static void AddConnectionId(IConnectionContext connectionContext)
    {
        ConnectionIdMap.Add(connectionContext.ConnectionId, connectionContext);
    }

    internal static void AddAuthorizationId(object authorizationId, string connectionId)
    {
        AuthorizationIdMap.Add(authorizationId, connectionId);
    }

    internal static void RemoveConnectionId(string connectionId)
    {
        var connectionContext = ConnectionIdMap.GetValueOrDefault(connectionId);
        if (connectionContext == null)
            return;

        if (connectionContext.IsAuthorized)
            AuthorizationIdMap.Remove(connectionContext.GetAuthorizationId<object>()!);

        ConnectionIdMap.Remove(connectionId);
    }

    internal static IConnectionContext? GetContextWithConnectionId(string connectionId)
    {
        return ConnectionIdMap.GetValueOrDefault(connectionId);
    }

    internal static IConnectionContext? GetContextWithAuthorizationId(object authorizationId)
    {
        var connectionId = AuthorizationIdMap.GetValueOrDefault(authorizationId);
        return connectionId == null ? null : ConnectionIdMap.GetValueOrDefault(connectionId);
    }
}