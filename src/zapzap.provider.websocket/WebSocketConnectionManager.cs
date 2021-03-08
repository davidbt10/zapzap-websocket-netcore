namespace zapzap.provider.websocket
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;
    using zapzap.common.Providers;

    public class WebSocketConnectionManager : IWebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            _sockets.TryGetValue(id, out var socket);
            return socket;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task RemoveSocket(string id)
        {
            if (id == null) return;

            _sockets.TryRemove(id, out var socket);

            if (socket.State != WebSocketState.Open) return;

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None).ConfigureAwait(false);
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
