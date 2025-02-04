using KarizmaPlatform.Connection.Core.Base;
using KarizmaPlatform.Connection.Server.Interfaces;

namespace KarizmaPlatform.Connection.Server.Connection;

internal class ConnectionContext(string connectionId) : IConnectionContext
{
    public string ConnectionId => connectionId;
    public bool IsAuthorized => authorizationId != null;
    public Vault Vault { get; } = new();

    private object? authorizationId;

    public void SetAuthorizationId(object authId)
    {
        authorizationId = authId;
        ConnectionContextRegistry.AddAuthorizationId(authorizationId, connectionId);
    }

    public T? GetAuthorizationId<T>()
    {
        if (!IsAuthorized)
            return default;

        return (T)authorizationId!;
    }
}