using KarizmaPlatform.Connection.Core.Base;
using KarizmaPlatform.Connection.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaPlatform.Connection.Server.Connection;

internal class ConnectionContext(HubCallerContext context) : IConnectionContext
{
    public string ConnectionId => context.ConnectionId;
    public bool IsAuthorized => authorizationId != null;
    public Vault Vault { get; } = new();

    private object? authorizationId;

    public void SetAuthorizationId(object authId)
    {
        authorizationId = authId;
        ConnectionContextRegistry.AddAuthorizationId(authorizationId, ConnectionId);
    }

    public T? GetAuthorizationId<T>()
    {
        if (!IsAuthorized)
            return default;

        return (T)authorizationId!;
    }

    public Task Disconnect()
    {
        // Task that completes when ConnectionAborted is triggered
        var tokenTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        if (context.ConnectionAborted.IsCancellationRequested)
            tokenTcs.SetResult();
        else
            context.ConnectionAborted.Register(() => tokenTcs.TrySetResult());

        // Task that completes when OnDisconnectedAsync finishes
        var disconnectTcs = ConnectionContextRegistry.AddDisconnectionSource(ConnectionId);

        // Trigger the abort
        context.Abort();

        // Return a combined Task that waits for both token and OnDisconnectedAsync
        return Task.WhenAll(tokenTcs.Task, disconnectTcs.Task);
    }

}