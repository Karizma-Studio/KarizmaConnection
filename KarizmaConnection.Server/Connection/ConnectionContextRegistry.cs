namespace KarizmaConnection.Server.Connection;

internal static class ConnectionContextRegistry
{
    private static readonly Dictionary<string, ConnectionContext> ConnectionIdMap = new();
    private static readonly Dictionary<object, string> AuthorizationIdMap = new();

    internal static void AddConnectionId(ConnectionContext connectionContext)
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

    internal static ConnectionContext? GetContextWithConnectionId(string connectionId)
    {
        return ConnectionIdMap.GetValueOrDefault(connectionId);
    }

    internal static ConnectionContext? GetContextWithAuthorizationId(object authorizationId)
    {
        var connectionId = AuthorizationIdMap.GetValueOrDefault(authorizationId);
        return connectionId == null ? null : ConnectionIdMap.GetValueOrDefault(connectionId);
    }
}