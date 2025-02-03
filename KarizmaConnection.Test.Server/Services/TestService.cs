using KarizmaPlatform.Connection.Server.Connection;
using KarizmaPlatform.Connection.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaConnection.Test.Server.Services;

public class TestService(ILogger<TestService> logger, IHub hub)
{
    public async Task NotifyUser(ConnectionContext connection)
    {
        logger.LogInformation($"Calling {connection.ConnectionId} ...");
        await hub.Send(connection.ConnectionId, "Hello", "Hello from server - TestService!");
    }
}