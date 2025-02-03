using System.Text.Json;
using KarizmaPlatform.Connection.Core.Base;
using KarizmaPlatform.Connection.Server.Connection;

namespace KarizmaPlatform.Connection.Server.Interfaces;

public interface IHub
{
    internal Task<Response<object?>> HandleAction(string address, params JsonElement[] body);
    public Task SendAll(string address, object body);
    public Task Send(string connectionId, string address, object body);
    public ConnectionContext? GetConnection(string connectionId);
    public ConnectionContext? GetAuthorizedConnection(object authorizationId);
}