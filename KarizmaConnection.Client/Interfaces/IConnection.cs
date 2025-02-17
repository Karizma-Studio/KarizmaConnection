using System;
using System.Threading.Tasks;
using KarizmaPlatform.Connection.Core.Base;

namespace KarizmaPlatform.Connection.Client.Interfaces
{
    public interface IConnection
    {
        public string? Id { get; }
        public bool IsConnected { get; }

        public event Action OnConnected;
        public event Action<Exception?> OnReconnecting;
        public event Action<Exception?> OnDisconnected;

        public Task Connect(string url);
        public Task Disconnect();

        public void On<T>(string address, Action<T> handler);

        public Task Send(string address, params object[] body);
        public Task<Response<TResponse>> Request<TResponse>(string address, params object[] body);
        public Task<Response<object?>> Request(string address, params object[] body);
    }
}