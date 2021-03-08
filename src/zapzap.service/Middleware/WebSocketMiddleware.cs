namespace zapzap.service.Middleware
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using zapzap.common.Providers;
    using zapzap.common.Services;

    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebsocketHandler _websocketHandler;

        public WebSocketMiddleware(RequestDelegate next, IWebsocketHandler websocketHandler)
        {
            _next = next;
            _websocketHandler = websocketHandler;
        }

        public async Task Invoke(HttpContext context, IChatHandler _chatHandler)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await _websocketHandler.OnConnected(socket);

            await Receive(socket, async (result, message) =>
            {
                await _chatHandler.ReceiveAsync(socket, message);
                return;
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, string> handleMessage)
        {
            while (socket.State == WebSocketState.Open)
            {
                ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
                string message = null;
                WebSocketReceiveResult result = null;
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                            ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);

                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            message = await reader.ReadToEndAsync();
                        }
                    }

                    handleMessage(result, message);
                }
                catch (WebSocketException e)
                {
                    if (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                    {
                        socket.Abort();
                    }
                }
            }

            await _websocketHandler.OnDisconnected(socket);
        }
    }
}
