using System;
using System.Threading.Tasks;

namespace KarizmaConnection.Client.Interfaces
{
    public interface IConnection : IAsyncDisposable
    {
        public string? Id { get; }

        public event Action OnConnected;
        public event Action<Exception?> OnReconnecting;
        public event Action<Exception?> OnDisconnected;

        public Task Connect(string url);
        public void On<T>(string address, Action<T> handler);
        public Task Send(string address, object body);
        public Task<TResponse> Request<TResponse>(string address, object body);
    }
}