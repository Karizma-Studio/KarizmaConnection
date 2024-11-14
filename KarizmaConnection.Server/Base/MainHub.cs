using System.Text.Json;
using KarizmaConnection.Core.Base;
using KarizmaConnection.Core.Constants;
using KarizmaConnection.Core.Exceptions;
using KarizmaConnection.Server.Connection;
using KarizmaConnection.Server.Extensions;
using KarizmaConnection.Server.Interfaces;
using KarizmaConnection.Server.RequestHandler;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaConnection.Server.Base;

internal class MainHub(
    ILogger<MainHub> logger,
    Options options,
    IEnumerable<BaseEventHandler> eventHandlers,
    IServiceProvider serviceProvider) : Hub, IHub
{
    public override async Task OnConnectedAsync()
    {
        var connectionContext = new ConnectionContext(Context.ConnectionId);
        ConnectionContextRegistry.AddConnectionId(connectionContext);

        foreach (var handler in eventHandlers)
        {
            handler.Initialize(this, connectionContext);
            await handler.OnConnected();
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionContext = ConnectionContextRegistry.GetContextWithConnectionId(Context.ConnectionId);

        foreach (var handler in eventHandlers)
        {
            handler.Initialize(this, connectionContext);
            await handler.OnDisconnected(exception);
        }

        ConnectionContextRegistry.RemoveConnectionId(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task<Response<object?>> HandleAction(string address, params JsonElement[] body)
    {
        try
        {
            if (!RequestHandlerRegistry.TryGetHandler(address, out var handlerAction))
                throw new KeyNotFoundException($"Address '{address}' not found.");

            //Check user authorization
            var user = ConnectionContextRegistry.GetContextWithConnectionId(Context.ConnectionId);
            if (handlerAction.NeedAuthorizedUser && !user.IsAuthorized)
                throw new Exception("Access denied.");

            //Get Handler instance and set context
            var handlerInstance = serviceProvider.GetRequiredService(handlerAction.HandlerType);
            ((BaseRequestHandler)handlerInstance).Initialize(this, user);

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

            if (handlerAction.ActionMethodInfo.ReturnType.IsGenericTaskType())
                return new Response<object?>(((dynamic)task).Result);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "MainHub HandleAction Error.");

            var innerException = ex.InnerException;
            while (innerException != null)
            {
                if (innerException is ResponseException responseException)
                    return new Response<object?>(null, new Error(responseException));
               
                innerException = innerException.InnerException;
            }

            var message = options.ReturnStackTraceOnError
                ? $"{ex.Message} \n {ex.StackTrace}"
                : "Internal Server Error";

            return new Response<object?>(null, new Error(options.DefaultHubResponseErrorCode, message));
        }

        return new Response<object?>(null);
    }
}