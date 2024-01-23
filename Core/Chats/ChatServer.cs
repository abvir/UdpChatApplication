using System.Net;
using System.Net.Sockets;
using Contracts;
using Entities;

namespace Core.Chats;

public sealed class ChatServer : ChatBase
{
    public ChatServer(IPEndPoint hostEndPoint) : base(new UdpClient(hostEndPoint))
    {
    }

    private readonly Dictionary<User, IPEndPoint> _clients = [];

    protected override async Task OnReceivedNewMessageHandler(Message message, IPEndPoint sender)
    {
         Console.WriteLine($"Client {message.Text} is connecting from {sender}");
        
        if (JoinRequest.Is(message.Text, out JoinRequest? request))
        {
            await HandleJoinNewUser(request!, sender);
            return;
        }



    }

    private async Task HandleJoinNewUser(JoinRequest joinRequest, IPEndPoint sender)
    {
        Console.WriteLine($"Joining {joinRequest.Username}");

        User user = new() { Username = joinRequest.Username };
        await SendAsync(new Message()
        {
            Text = new JoinResponse() { User = user }.Serialize(),
        }, sender);
        
        _clients[user] = sender;


        foreach (var client in _clients.Values)
        {
            await SendAsync(new Message()
            {
                Text = new OnlineResponse() { Users = _clients.Keys.ToList() }.Serialize(),
            }, sender);

            await SendAsync(new() { Text = $"{user.Username} joined." }, client);
        }
        Console.WriteLine($"Online users: {string.Join(", ", _clients.Keys.Select(x => x.Username))}");
    }

    public override async Task Run()
    {

        Console.WriteLine($"Server is running");
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
