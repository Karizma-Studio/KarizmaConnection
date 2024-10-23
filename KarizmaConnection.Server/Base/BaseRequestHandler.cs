using KarizmaConnection.Server.Interfaces;
using KarizmaConnection.Server.Users;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaConnection.Server.Base;

public abstract class BaseRequestHandler : IRequestHandler
{
    private Hub? hub;
    public string ConnectionId => hub!.Context.ConnectionId;
    public User User { get; internal set; } = null!;

    internal void Initialize(Hub contextHub, User user)
    {
        hub = contextHub;
        User = user;
    }

    public async Task SendAll(string address, object body)
    {
        await hub!.Clients.All.SendAsync(address, body);
    }

    public async Task Send(string connectionId, string address, object body)
    {
        await hub!.Clients.Client(connectionId).SendAsync(address, body);
    }
}