namespace KarizmaPlatform.Connection.Server.Config;

public class MainHubOptions
{
    public int DefaultHubResponseErrorCode { get; set; } = 500;
    public bool ReturnStackTraceOnError { get; set; } = true;
    public TimeSpan? KeepAliveInterval { get; set; } = TimeSpan.FromSeconds(15);
    public TimeSpan? ClientTimeoutInterval { get; set; }= TimeSpan.FromMinutes(2);
}