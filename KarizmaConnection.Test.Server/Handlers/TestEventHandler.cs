using KarizmaPlatform.Connection.Server.Attributes;
using KarizmaPlatform.Connection.Server.Base;

namespace KarizmaConnection.Test.Server.Handlers;

[EventHandler]
public class TestEventHandler : BaseEventHandler
{
    public override async Task OnConnected()
    {
        await Task.Delay(500);
        Console.WriteLine("=====> Connected " + ConnectionContext.ConnectionId);
    }

    public override async Task OnDisconnected(Exception? exception)
    {
        await Task.Delay(500);
        Console.WriteLine("=====> Disconnected " + ConnectionContext.ConnectionId);
    }
}