using KarizmaPlatform.Connection.Server.Connection;
using KarizmaPlatform.Connection.Server.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaConnection.Test.Server.Services;

public class TestService(ILogger<TestService> logger, IMainHubContext mainHub)
{
    public async Task NotifyUser(IConnectionContext connection)
    {
        logger.LogInformation($"Calling {connection.ConnectionId} ...");
        await mainHub.Send(connection.ConnectionId, "Hello", "Hello from server - TestService!");
    }
}