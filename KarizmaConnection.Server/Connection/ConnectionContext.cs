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

    public void Disconnect()
    {
        context.Abort();
    }
}