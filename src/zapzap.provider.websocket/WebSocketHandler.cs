namespace zapzap.provider.websocket
{
    using System;
    using System.Collections.Generic;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using zapzap.common;
    using zapzap.common.Providers;

    public class WebSocketHandler : IWebsocketHandler
    {
        public IWebSocketConnectionManager _webSocketConnectionManager { get; set; }

        public WebSocketHandler(IWebSocketConnectionManager webSocketConnectionManager)
        {
            _webSocketConnectionManager = webSocketConnectionManager;
        }

        public string GetSocketID(WebSocket socket)
        {
            return _webSocketConnectionManager.GetId(socket);
        }

        public async Task OnConnected(WebSocket socket)
        {
            _webSocketConnectionManager.AddSocket(socket);
            await SendMessageAsync(socket, Constants.Messages.WelcomeMessage);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await _webSocketConnectionManager.RemoveSocket(_webSocketConnectionManager.GetId(socket));
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            var encodedMessage = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(buffer: new ArraySegment<byte>(array: encodedMessage, offset: 0, count: encodedMessage.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            await SendMessageAsync(_webSocketConnectionManager.GetSocketById(socketId), message);
        }
        
        public async Task SendMessageToGroupAsync(List<string> sockets, string message)
        {
            if (sockets != null)
            {
                foreach (var socket in sockets)
                {
                    await SendMessageAsync(socket, message);
                }
            }
        }

        public async Task RemoveSocket(string id)
        {
            await _webSocketConnectionManager.RemoveSocket(id);
        }
    }
}
