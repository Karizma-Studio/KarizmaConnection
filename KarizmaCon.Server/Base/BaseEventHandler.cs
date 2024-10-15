using KarizmaCon.Server.Interfaces;

namespace KarizmaCon.Server.Base;

public abstract class BaseEventHandler : BaseRequestHandler, IEventHandler
{
    public abstract Task OnConnected();
    public abstract Task OnDisconnected(Exception? exception);
}