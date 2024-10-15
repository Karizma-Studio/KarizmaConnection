using System;
using System.Text.Json;
using System.Threading.Tasks;
using KarizmaCon.Client.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace KarizmaCon.Client.Base
{
    public class BaseConnection : IConnection
    {
        public string? Id => hubConnection?.ConnectionId;
        private const string MainHandlerMethodName = "HandleAction";

        private HubConnection? hubConnection;
        private string? lastConnectedUrl;

        public event Action OnConnected = delegate { };
        public event Action<Exception?> OnDisconnected = delegate { };
        public event Action<Exception?> OnReconnecting = delegate { };

        private readonly ILogger? logger;

        public BaseConnection(ILogger? logger = null)
        {
            this.logger = logger;
        }

        public async Task Connect(string url)
        {
            if (hubConnection != null)
                await hubConnection.DisposeAsync();

            logger?.LogInformation($"Start connecting to: {url}");

            hubConnection = CreateHubConnection(url);

            await hubConnection!.StartAsync();

            OnConnected.Invoke();

            lastConnectedUrl = url;
            logger?.LogInformation("Connection established.");
        }

        private HubConnection CreateHubConnection(string address)
        {
            var connectionBuilder = new HubConnectionBuilder()
                .WithUrl(address)
                .WithAutomaticReconnect();

            var connection = connectionBuilder.Build();

            connection.Reconnecting += HubConnectionOnReconnecting;
            connection.Reconnected += HubConnectionOnReconnected;
            connection.Closed += HubConnectionOnClosed;


            return connection;
        }

        private Task HubConnectionOnReconnecting(Exception? exception)
        {
            logger?.LogWarning(exception, "Connection lost. reconnecting ...");
            OnReconnecting.Invoke(exception);
            return Task.CompletedTask;
        }

        private Task HubConnectionOnReconnected(string? newConnectionId)
        {
            logger?.LogInformation($"Connection established. New Connection ID: {newConnectionId}");
            OnConnected.Invoke();
            return Task.CompletedTask;
        }

        private Task HubConnectionOnClosed(Exception? exception)
        {
            logger?.LogWarning(exception, "Connection Closed.");
            OnDisconnected.Invoke(exception);
            return Task.CompletedTask;
        }

        private async Task CheckConnection()
        {
            if (hubConnection == null || lastConnectedUrl == null)
                throw new Exception("Connection is not initialized.");

            if (hubConnection.State == HubConnectionState.Disconnected)
                await Connect(lastConnectedUrl);
        }

        private async Task Disconnect()
        {
            if (hubConnection == null)
                return;

            await hubConnection.DisposeAsync();
        }

        public void On<T>(string address, Action<T> handler)
        {
            hubConnection?.On(address, handler);
        }

        public async Task Send(string address, object body)
        {
            await CheckConnection();
            _ = hubConnection!.InvokeAsync(MainHandlerMethodName, address, body);
        }

        public async Task<T> Request<T>(string address, object body)
        {
            await CheckConnection();
            var result = await hubConnection!.InvokeAsync<JsonElement>(MainHandlerMethodName, address, body);
            return result.Deserialize<T>()!;
        }

        public async ValueTask DisposeAsync()
        {
            await Disconnect();
        }
    }
}