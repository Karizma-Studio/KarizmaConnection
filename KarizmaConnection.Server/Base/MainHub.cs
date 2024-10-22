using System.Text.Json;
using KarizmaConnection.Core.Base;
using KarizmaConnection.Core.Exceptions;
using KarizmaConnection.Server.Interfaces;
using KarizmaConnection.Server.RequestHandler;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaConnection.Server.Base;

internal class MainHub(
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

    public async Task<Response<object?>> HandleAction(string address, JsonElement body)
    {
        try
        {
            if (!requestHandlerRegistry.TryGetHandler(address, out var handlerAction))
                throw new KeyNotFoundException($"Address '{address}' not found.");

            var parameterType = handlerAction.ActionMethodInfo.GetParameters()[0].ParameterType;
            var bodyObject = body.Deserialize(parameterType);

            var handlerInstance = serviceProvider.GetRequiredService(handlerAction.HandlerType);
            ((BaseRequestHandler)handlerInstance).SetContext(this);

            var result = handlerAction.ActionMethodInfo.Invoke(handlerInstance, [bodyObject]);

            if (result is not Task task)
                return new Response<object?>(result);

            await task;

            if (handlerAction.ActionMethodInfo.ReturnType.IsGenericType
                && handlerAction.ActionMethodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                return new Response<object?>(((dynamic)task).Result);
        }
        catch (Exception ex)
        {
            Error? error = null;

            if (ex.InnerException is ResponseException innerException)
                error = new Error(innerException.Code, innerException.Message);

            return new Response<object?>(null, error ?? new Error(100, ex.Message)); //TODO Default error code
        }

        return new Response<object?>(null);
    }
}