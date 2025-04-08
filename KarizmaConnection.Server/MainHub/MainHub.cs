using System.Text.Json;
using KarizmaPlatform.Connection.Core.Base;
using KarizmaPlatform.Connection.Core.Constants;
using KarizmaPlatform.Connection.Core.Exceptions;
using KarizmaPlatform.Connection.Server.Base;
using KarizmaPlatform.Connection.Server.Config;
using KarizmaPlatform.Connection.Server.Connection;
using KarizmaPlatform.Connection.Server.Extensions;
using KarizmaPlatform.Connection.Server.Interfaces;
using KarizmaPlatform.Connection.Server.RequestHandler;
using Microsoft.AspNetCore.SignalR;

namespace KarizmaPlatform.Connection.Server.MainHub;

internal class MainHub(
    ILogger<MainHub> logger,
    MainHubOptions mainHubOptions,
    IEnumerable<BaseEventHandler> eventHandlers,
    IMainHubContext mainHubContext,
    IServiceProvider serviceProvider) : Hub, IMainHub
{
    public override async Task OnConnectedAsync()
    {
        var connectionContext = new ConnectionContext(Context);
        ConnectionContextRegistry.AddConnectionId(connectionContext);

        foreach (var handler in eventHandlers)
        {
            handler.Initialize(mainHubContext, connectionContext);
            await handler.OnConnected();
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionContext = ConnectionContextRegistry.GetContextWithConnectionId(Context.ConnectionId);

        foreach (var handler in eventHandlers)
        {
            handler.Initialize(mainHubContext, connectionContext!);
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

            //Check user existence and authorization
            var user = ConnectionContextRegistry.GetContextWithConnectionId(Context.ConnectionId);

            if (user == null)
                throw new Exception("User is null - Connection ID: " + Context.ConnectionId);

            if (handlerAction.NeedAuthorizedUser && !user.IsAuthorized)
                throw new Exception("User is not authorized for this handler - Address: " + address);

            //Get Handler instance and set context
            var handlerInstance = serviceProvider.GetRequiredService(handlerAction.HandlerType);
            ((BaseRequestHandler)handlerInstance).Initialize(mainHubContext, user);

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
            var innerException = ex;
            while (innerException != null)
            {
                if (innerException is ResponseException responseException)
                {
                    logger.LogWarning(ex, "[MainHub | HandleAction] Got Response Exception.");
                    return new Response<object?>(null, new Error(responseException));
                }

                innerException = innerException.InnerException;
            }

            var message = mainHubOptions.ReturnStackTraceOnError
                ? $"{ex.Message} \n {ex.StackTrace}"
                : "Internal Server Error";

            logger.LogCritical(ex, "[MainHub | HandleAction] Got Unhandled Exception.");
            return new Response<object?>(null, new Error(mainHubOptions.DefaultHubResponseErrorCode, message));
        }

        return new Response<object?>(null);
    }
}