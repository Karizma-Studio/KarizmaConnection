using System.Collections.Concurrent;
using KarizmaPlatform.Connection.Server.Interfaces;

namespace KarizmaPlatform.Connection.Server.Connection;

internal static class ConnectionContextRegistry
{
    private static readonly ConcurrentDictionary<string, IConnectionContext> ConnectionIdMap = new();
    private static readonly ConcurrentDictionary<object, string> AuthorizationIdMap = new();
    private static readonly ConcurrentDictionary<string, TaskCompletionSource> DisconnectTasks = new();

    internal static void AddConnectionId(IConnectionContext connectionContext)
    {
        ConnectionIdMap.TryAdd(connectionContext.ConnectionId, connectionContext);
    }

    internal static void AddAuthorizationId(object authorizationId, string connectionId)
    {
        AuthorizationIdMap.TryAdd(authorizationId, connectionId);
    }

    internal static void RemoveConnectionId(string connectionId)
    {
        var connectionContext = ConnectionIdMap.GetValueOrDefault(connectionId);
        if (connectionContext == null)
            return;

        if (connectionContext.IsAuthorized)
            AuthorizationIdMap.Remove(connectionContext.GetAuthorizationId<object>()!, out _);

        ConnectionIdMap.Remove(connectionId, out _);
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

    internal static TaskCompletionSource AddDisconnectionSource(string connectionId)
    {
        var newTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        return DisconnectTasks.GetOrAdd(connectionId, newTcs);
    }
    
    internal static void TriggerDisconnectionSource(string connectionId)
    {
        if (DisconnectTasks.TryRemove(connectionId, out var tcs))
            tcs.SetResult();
    }
}