using System.Text.Json;
using KarizmaConnection.Core.Base;
using KarizmaConnection.Core.Constants;
using KarizmaConnection.Core.Exceptions;
using KarizmaConnection.Server.Interfaces;
using KarizmaConnection.Server.RequestHandler;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaConnection.Server.Base;

internal class MainHub(
    ILogger<MainHub> logger,
    RequestHandlerRegistry requestHandlerRegistry,
    Options options,
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

    public async Task<Response<object?>> HandleAction(string address, params JsonElement[] body)
    {
        try
        {
            if (!requestHandlerRegistry.TryGetHandler(address, out var handlerAction))
                throw new KeyNotFoundException($"Address '{address}' not found.");

            //Get Handler instance and set context
            var handlerInstance = serviceProvider.GetRequiredService(handlerAction.HandlerType);
            ((BaseRequestHandler)handlerInstance).SetContext(this);

            //Initialize action input parameters
            List<object?> actionInputParams = [];
            var actionInputParamsInfo = handlerAction.ActionMethodInfo.GetParameters();

            for (var i = 0; i < actionInputParamsInfo.Length; i++)
                actionInputParams.Add(body[i].Deserialize(actionInputParamsInfo[i].ParameterType,
                    JsonSerializerConstants.JsonSerializerOptions));

            //Get the action result
            var result = handlerAction.ActionMethodInfo.Invoke(handlerInstance, actionInputParams.ToArray());

            if (result is not Task task)
                return new Response<object?>(result);

            await task;

            if (handlerAction.ActionMethodInfo.ReturnType.IsGenericType
                && handlerAction.ActionMethodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                return new Response<object?>(((dynamic)task).Result);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "MainHub HandleAction Error.");
            Error? error = null;

            if (ex.InnerException is ResponseException responseException)
                error = new Error(responseException.Code, responseException.Message);

            return new Response<object?>(null, error ?? new Error(options.DefaultHubResponseErrorCode, ex.Message));
        }

        return new Response<object?>(null);
    }
}