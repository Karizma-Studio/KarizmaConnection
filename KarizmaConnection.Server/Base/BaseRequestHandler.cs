using KarizmaPlatform.Connection.Server.Interfaces;

namespace KarizmaPlatform.Connection.Server.Base;

public abstract class BaseRequestHandler
{
    protected IMainHubContext MainHub { get; private set; } = null!;
    protected IConnectionContext ConnectionContext { get; private set; } = null!;

    internal void Initialize(IMainHubContext mainHub, IConnectionContext connectionContext)
    {
        MainHub = mainHub;
        ConnectionContext = connectionContext;
    }
}