using System;
using System.Threading.Tasks;
using KarizmaConnection.Core.Base;

namespace KarizmaConnection.Client.Interfaces
{
    public interface IConnection
    {
        public string? Id { get; }

        public event Action OnConnected;
        public event Action<Exception?> OnReconnecting;
        public event Action<Exception?> OnDisconnected;

        public Task Connect(string url);
        public Task Disconnect();

        public void On<T>(string address, Action<T> handler);

        public Task Send(string address, object body);
        public Task<Response<TResponse>> Request<TResponse>(string address, object body);
    }
}