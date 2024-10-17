using KarizmaConnection.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaConnection.Server.Base;

public abstract class BaseRequestHandler : IRequestHandler
{
    private Hub? hub;
    public string ConnectionId => hub!.Context.ConnectionId;

    internal void SetContext(Hub contextHub)
    {
        hub = contextHub;
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