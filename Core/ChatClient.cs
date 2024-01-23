using System.Net;
using System.Net.Sockets;
using Entities;

namespace Core;

public sealed class ChatClient : ChatBase
{
    private readonly IPEndPoint _hostEndPoint;
    public ChatClient(IPEndPoint hostEndPoint) : base(new UdpClient(0))
    {
        _hostEndPoint = hostEndPoint;
    }

    protected override async Task OnReceivedNewMessageHandler(Message message, IPEndPoint sender)
    {
        await Task.CompletedTask;
    }

    public override async Task Run()
    {
        #pragma warning disable CS4014 
        Task.Run(_udpProvider.ReceiveAsync);
        #pragma warning restore CS4014 
     
        string input = Console.ReadLine() ?? string.Empty;
        while (input != string.Empty)
        {
            await SendAsync(new() { Text = input, RecipientId = Guid.Empty, SenderId = Guid.Empty }, _hostEndPoint);
            
            input = Console.ReadLine() ?? string.Empty;
        }
        await _udpProvider.CancelAsync();
    }
}
