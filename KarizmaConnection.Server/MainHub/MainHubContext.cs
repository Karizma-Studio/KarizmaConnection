using KarizmaPlatform.Connection.Server.Connection;
using KarizmaPlatform.Connection.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaPlatform.Connection.Server.MainHub;

internal class MainHubContext(IHubContext<MainHub> mainHub) : IMainHubContext
{
    public async Task SendAll(string address, object body)
    {
        await mainHub.Clients.All.SendAsync(address, body);
    }

    public async Task Send(string connectionId, string address, object body)
    {
        await mainHub.Clients.Client(connectionId).SendAsync(address, body);
    }

    public IConnectionContext? GetConnection(string connectionId)
    {
        return ConnectionContextRegistry.GetContextWithConnectionId(connectionId);
    }

    public IConnectionContext? GetAuthorizedConnection(object authorizationId)
    {
        return ConnectionContextRegistry.GetContextWithAuthorizationId(authorizationId);
    }
}