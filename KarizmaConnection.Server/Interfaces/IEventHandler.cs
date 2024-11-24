namespace KarizmaPlatform.Connection.Server.Interfaces;

public interface IEventHandler
{
    public Task OnConnected();
    public Task OnDisconnected(Exception? exception);
}