using System.Net;
using System.Net.Sockets;
using Entities;

namespace Core;

public sealed class ChatServer : ChatBase
{
    public ChatServer(IPEndPoint hostEndPoint) : base(new UdpClient(hostEndPoint))
    {
    }

    private readonly HashSet<IPEndPoint> _clients = [];

    protected override async Task OnReceivedNewMessageHandler(Message message, IPEndPoint sender)
    {       
        _clients.Add(sender);
        foreach (var client in _clients.Where(x => x != sender))
        {
            await SendAsync(message, client);
        }
    }

    public override async Task Run()
    {
        #pragma warning disable CS4014 
        Task.Run(_udpProvider.ReceiveAsync);
        #pragma warning restore CS4014 
        
        string input = Console.ReadLine() ?? string.Empty;
        while (input != string.Empty)
        {            
            input = Console.ReadLine() ?? string.Empty;
        }
        await _udpProvider.CancelAsync();
    }
}
