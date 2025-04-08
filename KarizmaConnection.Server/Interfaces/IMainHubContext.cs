namespace KarizmaPlatform.Connection.Server.Interfaces;

public interface IMainHubContext
{
    public Task SendAll(string address, object body);
    public Task Send(string connectionId, string address, object body);
    public IConnectionContext? GetConnection(string connectionId);
    public IConnectionContext? GetAuthorizedConnection(object authorizationId);
}