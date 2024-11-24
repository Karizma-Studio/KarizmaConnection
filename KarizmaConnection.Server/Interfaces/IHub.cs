using System.Text.Json;
using KarizmaPlatform.Connection.Core.Base;

namespace KarizmaPlatform.Connection.Server.Interfaces;

internal interface IHub
{
    internal Task<Response<object?>> HandleAction(string address, params JsonElement[] body);
}