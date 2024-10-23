namespace KarizmaConnection.Server.Users;

internal class UserRegistry
{
    private readonly Dictionary<string, User> users = new();

    public void Add(string key, User user)
    {
        users.Add(key, user);
    }

    public void Remove(string connectionId)
    {
        users.Remove(connectionId);
    }

    public User Get(string connectionId)
    {
        return users.GetValueOrDefault(connectionId)!;
    }
}