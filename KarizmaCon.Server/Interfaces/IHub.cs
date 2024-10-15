using System.Text.Json;

namespace KarizmaCon.Server.Interfaces;

internal interface IHub
{
    internal Task<object?> HandleAction(string address, JsonElement body);
}