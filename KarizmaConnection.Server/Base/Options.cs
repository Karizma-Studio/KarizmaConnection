namespace KarizmaConnection.Server.Base;

public class Options
{
    public int DefaultHubResponseErrorCode { get; set; } = 500;
    public bool ReturnStackTraceOnError { get; set; } = true;
}