namespace KarizmaConnection.Server.Base;

public class Options(int defaultHubResponseErrorCode)
{
    public int DefaultHubResponseErrorCode { get; private set; } = defaultHubResponseErrorCode;
}