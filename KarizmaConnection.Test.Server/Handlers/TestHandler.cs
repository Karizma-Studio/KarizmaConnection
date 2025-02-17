using KarizmaConnection.Test.Server.Services;
using KarizmaPlatform.Connection.Core.Exceptions;
using KarizmaPlatform.Connection.Server.Attributes;
using KarizmaPlatform.Connection.Server.Base;

namespace KarizmaConnection.Test.Server.Handlers;

[RequestHandler("Test")]
[CustomEventDoc("test/hello3", typeof(string), "This is my summary 3", "This is my description")]
public class TestHandler(TestService testService) : BaseRequestHandler
{
    [Action("GetHello", needAuthorizedUser: false)]
    public async Task<string> GetHello(string name)
    {
        await Task.Delay(500);
        return $"Hello dear {name}";
    }

    [Action("SendHelloToAll", needAuthorizedUser: false)]
    [CustomEventDoc("test/hello", typeof(string), "This is my summary", "This is my description")]
    [CustomEventDoc("test/hello2", typeof(string), "This is my summary 2", "This is my description 2")]
    private async Task SendHelloToAll()
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
    
    
    [Action("Error", needAuthorizedUser: false)]
    public async Task<bool> Error()
    {
        await Task.Delay(500);
        throw new ResponseException(401, "this is my custom error message.");
    }
}