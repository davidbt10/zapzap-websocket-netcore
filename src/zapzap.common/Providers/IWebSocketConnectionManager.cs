namespace zapzap.common.Providers
{
    using System.Collections.Concurrent;
    using System.Net.WebSockets;
    using System.Threading.Tasks;

    public interface IWebSocketConnectionManager
    {
        string GetId(WebSocket socket);
        void AddSocket(WebSocket socket);
        Task RemoveSocket(string id);
        ConcurrentDictionary<string, WebSocket> GetAll();
        WebSocket GetSocketById(string id);
    }
}
