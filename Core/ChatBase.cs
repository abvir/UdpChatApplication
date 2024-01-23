using System.Net;
using System.Net.Sockets;
using Entities;

namespace Core;

public abstract class ChatBase
{
    protected readonly UdpProvider _udpProvider;
    protected ChatBase(UdpClient client)
    {

        _udpProvider = new UdpProvider(client);
        _udpProvider.OnReceivedNewMessage += OnReceivedNewMessageHandler;
    }

    protected abstract Task OnReceivedNewMessageHandler(Message message, IPEndPoint sender);
    public abstract Task Run();
    protected async Task SendAsync(Message message, IPEndPoint remote)
    {
        await _udpProvider.SendAsync(message, remote);
    }
}
