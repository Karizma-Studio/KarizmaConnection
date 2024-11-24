using KarizmaPlatform.Connection.Server.Connection;
using KarizmaPlatform.Connection.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaPlatform.Connection.Server.Base;

public abstract class BaseRequestHandler : IRequestHandler
{
    private Hub? hub;
    public ConnectionContext ConnectionContext { get; private set; } = null!;

    internal void Initialize(Hub contextHub, ConnectionContext connectionContext)
    {
        hub = contextHub;
        ConnectionContext = connectionContext;
    }

    public async Task SendAll(string address, object body)
    {
        await hub!.Clients.All.SendAsync(address, body);
    }

    public async Task Send(string connectionId, string address, object body)
    {
        await hub!.Clients.Client(connectionId).SendAsync(address, body);
    }

    public ConnectionContext? GetConnectionContextWithConnectionId(string connectionId)
    {
        return ConnectionContextRegistry.GetContextWithConnectionId(connectionId);
    }

    public ConnectionContext? GetConnectionContextWithAuthorizationId(object authorizationId)
    {
        return ConnectionContextRegistry.GetContextWithAuthorizationId(authorizationId);
    }
}