using KarizmaConnection.Core.Base;

namespace KarizmaConnection.Server.Users;

public class User
{
    public bool IsAuthorized { get; private set; } = false;
    public readonly Vault Vault = new();

    public void SetAuthorized()
    {
        IsAuthorized = true;
    }
}