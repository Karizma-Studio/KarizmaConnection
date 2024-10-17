using System.Text.Json;
using KarizmaConnection.Server.Interfaces;
using KarizmaConnection.Server.RequestHandler;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaConnection.Server.Base;

internal class BaseHub(
    RequestHandlerRegistry requestHandlerRegistry,
    IEnumerable<BaseEventHandler> eventHandlers,
    IServiceProvider serviceProvider) : Hub, IHub
{
    public override async Task OnConnectedAsync()
    {
        foreach (var handler in eventHandlers)
        {
            handler.SetContext(this);
            await handler.OnConnected();
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        foreach (var handler in eventHandlers)
        {
            handler.SetContext(this);
            await handler.OnDisconnected(exception);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task<object?> HandleAction(string address, JsonElement body)
    {
        if (!requestHandlerRegistry.TryGetHandler(address, out var handlerAction))
            throw new KeyNotFoundException($"Address '{address}' not found.");

        var parameterType = handlerAction.ActionMethodInfo.GetParameters()[0].ParameterType;
        var bodyObject = body.Deserialize(parameterType);

        var handlerInstance = serviceProvider.GetRequiredService(handlerAction.HandlerType);
        ((BaseRequestHandler)handlerInstance).SetContext(this);

        var result = handlerAction.ActionMethodInfo.Invoke(handlerInstance, [bodyObject]);

        if (result is not Task task)
            return result;

        await task;

        if (handlerAction.ActionMethodInfo.ReturnType.IsGenericType
            && handlerAction.ActionMethodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            return ((dynamic)task).Result;

        return null;
    }
}