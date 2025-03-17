using KarizmaPlatform.Connection.Core.Base;

namespace KarizmaPlatform.Connection.Server.Interfaces;

public interface IConnectionContext
{
    public string ConnectionId { get; }
    public bool IsAuthorized { get; }
    public Vault Vault { get; }
    public void SetAuthorizationId(object authId);
    public T? GetAuthorizationId<T>();
    public void Disconnect();
}