namespace zapzap.common.Providers
{
    using System.Collections.Generic;
    using System.Net.WebSockets;
    using System.Threading.Tasks;

    public interface IWebsocketHandler
    {
        Task OnConnected(WebSocket socket);
        Task OnDisconnected(WebSocket socket);
        string GetSocketID(WebSocket socket);
        Task SendMessageToGroupAsync(List<string> sockets, string message);
        Task SendMessageAsync(string socketId, string message);
        Task RemoveSocket(string id);
    }
}
