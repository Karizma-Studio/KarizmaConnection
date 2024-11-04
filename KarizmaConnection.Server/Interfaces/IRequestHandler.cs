using KarizmaConnection.Server.Connection;

namespace KarizmaConnection.Server.Interfaces;

internal interface IRequestHandler
{
    public ConnectionContext ConnectionContext { get; }
    public Task SendAll(string address, object body);
    public Task Send(string connectionId, string address, object body);
}