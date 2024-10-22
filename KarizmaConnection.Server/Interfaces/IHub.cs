using System.Text.Json;
using KarizmaConnection.Core.Base;

namespace KarizmaConnection.Server.Interfaces;

internal interface IHub
{
    internal Task<Response<object?>> HandleAction(string address, JsonElement body);
}