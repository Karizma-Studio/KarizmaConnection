using KarizmaPlatform.Connection.Server.Connection;
using KarizmaPlatform.Connection.Server.Interfaces;

namespace KarizmaPlatform.Connection.Server.Base;

public abstract class BaseRequestHandler
{
    protected IHub MainHub { get; private set; } = null!;
    public ConnectionContext ConnectionContext { get; private set; } = null!;

    internal void Initialize(IHub mainHub, ConnectionContext connectionContext)
    {
        MainHub = mainHub;
        ConnectionContext = connectionContext;
    }
}