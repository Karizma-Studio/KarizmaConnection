namespace KarizmaPlatform.Connection.Server.Config;

public class MainHubOptions
{
    public int DefaultHubResponseErrorCode { get; set; } = 500;
    public bool ReturnStackTraceOnError { get; set; } = true;
}