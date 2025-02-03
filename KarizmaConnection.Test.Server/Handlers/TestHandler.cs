using KarizmaConnection.Test.Server.Services;
using KarizmaPlatform.Connection.Server.Attributes;
using KarizmaPlatform.Connection.Server.Base;

namespace KarizmaConnection.Test.Server.Handlers;

[RequestHandler("Test")]
public class TestHandler(TestService testService) : BaseRequestHandler
{
    [Action("GetHello", needAuthorizedUser: false)]
    public async Task<string> GetHello(string name)
    {
        await Task.Delay(500);
        return $"Hello dear {name}";
    }

    [Action("SendHelloToAll", needAuthorizedUser: false)]
    public async Task SendHelloToAll()
    {
        await Task.Delay(500);
        await MainHub.SendAll("Hello", "Hello from server");
    }

    [Action("NotifyMe", needAuthorizedUser: false)]
    public async Task NotifyMe()
    {
        await Task.Delay(500);
        await testService.NotifyUser(ConnectionContext);
    }
}