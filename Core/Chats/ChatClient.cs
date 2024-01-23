using System.Data.Common;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Contracts;
using Entities;

namespace Core.Chats;

public sealed class ChatClient : ChatBase
{
    private readonly IPEndPoint _hostEndPoint;
    private User? _user;
    private List<User> _users  = []; 
    private readonly string _username;
    public ChatClient(IPEndPoint hostEndPoint, string username) : base(new UdpClient(34000))
    {
        _hostEndPoint = hostEndPoint;
        _username = username;
    }

    protected override async Task OnReceivedNewMessageHandler(Message message, IPEndPoint sender)
    {
        Console.WriteLine($"Message {message.Text} from {sender.Address}:{sender.Port}");
        
        
        if(JoinResponse.Is(message.Text, out JoinResponse? joinResponse))
        {
            HandleJoinCommand(joinResponse!);
            return;
        }
        if(OnlineResponse.Is(message.Text, out OnlineResponse? onlineResponse))
        {
            HandleOnlineCommand(onlineResponse!);
            return;
        }
        
        Console.WriteLine($"{_user?.Username}: {message.Text}");

        await Task.CompletedTask;
    }

    private void HandleOnlineCommand(OnlineResponse onlineResponse)
    {
        _users = onlineResponse.Users;
    }

    private void HandleJoinCommand(JoinResponse response)
    {
        _user= response.User;
    }

    public override async Task Run()
    {
#pragma warning disable CS4014
        Task.Run(_udpProvider.ReceiveAsync);
#pragma warning restore CS4014

        await Register();    

        await Conversation();
    }

    private async Task Register()
    {
        Console.WriteLine($"Client {_username} is connecting to {_hostEndPoint}");
        await SendAsync(new()
            {
                Text = new JoinRequest() { Username = _username}.Serialize(),
                RecipientId = Guid.Empty,
                SenderId = Guid.Empty
            }, _hostEndPoint);

        //  await Task.Delay(1000);   
        //  Console.WriteLine($"User {_user!.Username} was registered with Id {_user.Id}");
    }

    private async Task Conversation()
    {
        string input = Console.ReadLine() ?? string.Empty;
        while (input != string.Empty)
        {
            User? recipient = _users.FirstOrDefault(x=>input.StartsWith(x.Username));
            var recipientId = recipient is not null ? recipient.Id : Guid.Empty; 
            
            await SendAsync(new()
            {
                Text = input,
                RecipientId = recipientId,
                SenderId =  Guid.Empty
            }, _hostEndPoint);

            input = Console.ReadLine() ?? string.Empty;
        }
        await _udpProvider.CancelAsync();
    }
}
