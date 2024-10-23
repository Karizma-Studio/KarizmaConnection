using KarizmaConnection.Server.Users;

namespace KarizmaConnection.Server.Interfaces;

internal interface IRequestHandler
{
    public string ConnectionId { get; }
    public User User { get; }
    public Task SendAll(string address, object body);
    public Task Send(string connectionId, string address, object body);
}