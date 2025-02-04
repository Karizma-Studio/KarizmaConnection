using System.Reflection;
using KarizmaPlatform.Connection.Server.Attributes;
using KarizmaPlatform.Connection.Server.Base;
using KarizmaPlatform.Connection.Server.Config;
using KarizmaPlatform.Connection.Server.Interfaces;
using KarizmaPlatform.Connection.Server.MainHub;
using KarizmaPlatform.Connection.Server.RequestHandler;

namespace KarizmaPlatform.Connection.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddKarizmaConnection(this IServiceCollection services,
        MainHubOptions? hubOptions = null)
    {
        services.AddSignalR();

        services.AddEventHandlers();
        services.AddRequestHandlers();

        hubOptions ??= new MainHubOptions();
        services.AddSingleton(hubOptions);
        services.AddSingleton<IMainHubContext, MainHubContext>();
    }

    private static void AddRequestHandlers(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<RequestHandlerAttribute>() != null);

            foreach (var handlerType in handlerTypes)
            {
                services.AddTransient(handlerType);

                var handlerAttribute = handlerType.GetCustomAttribute<RequestHandlerAttribute>();

                var methodsWithActions = handlerType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .Where(m => m.GetCustomAttribute<ActionAttribute>() != null);

                foreach (var method in methodsWithActions)
                {
                    var actionAttribute = method.GetCustomAttribute<ActionAttribute>();

                    var address = $"{handlerAttribute!.Route}/{actionAttribute!.Route}".ToLowerInvariant();

                    RequestHandlerRegistry.AddHandler(address,
                        new RequestHandlerAction(address, handlerType, method, actionAttribute.NeedAuthorizedUser));
                }
            }
        }
    }
    
    private static void AddEventHandlers(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<EventHandlerAttribute>() != null);

            foreach (var handlerType in handlerTypes)
                services.AddTransient(typeof(BaseEventHandler), handlerType);
        }
    }
}