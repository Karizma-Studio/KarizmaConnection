using KarizmaConnection.Core.Base;

namespace KarizmaConnection.Server.Connection;

public class ConnectionContext(string connectionId)
{
    public string ConnectionId => connectionId;
    private object? authorizationId;
    public bool IsAuthorized => authorizationId != null;
    public readonly Vault Vault = new();

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