using KarizmaPlatform.Connection.Server.Interfaces;

namespace KarizmaPlatform.Connection.Server.Base;

public abstract class BaseEventHandler : BaseRequestHandler
{
    public abstract Task OnConnected();
    public abstract Task OnDisconnected(Exception? exception);
}