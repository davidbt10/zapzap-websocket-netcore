namespace zapzap.common.Services
{
    using System.Net.WebSockets;
    using System.Threading.Tasks;

    public interface IChatHandler
    {
        Task ReceiveAsync(WebSocket socket, string text);
    }
}
