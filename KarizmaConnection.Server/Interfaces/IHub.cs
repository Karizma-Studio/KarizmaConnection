using System.Text.Json;

namespace KarizmaConnection.Server.Interfaces;

internal interface IHub
{
    internal Task<object?> HandleAction(string address, JsonElement body);
}