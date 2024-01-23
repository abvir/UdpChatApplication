using System.Net;
using System.Net.Sockets;
using Core.Extensions;
using Entities;

namespace Core;

public class UdpProvider
{
    private readonly UdpClient _client;
    protected CancellationToken _cancellationToken;
    protected CancellationTokenSource _cancellationTokenSource;

    public delegate Task ReceivedNewMessage(Message message, IPEndPoint sender);
    public delegate Task ErrorWasThrown(Exception exc);
    public event ReceivedNewMessage? OnReceivedNewMessage;
    public event ErrorWasThrown? OnErrorThrown;

    public UdpProvider(UdpClient client)
    {
        _client = client;
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
    }

    public async Task ReceiveAsync()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            try
            {
                UdpReceiveResult result = await _client.ReceiveAsync(_cancellationToken);
                Message? message = result.Buffer.ToMessage() ?? throw new Exception("Message broken");
                OnReceivedNewMessage?.Invoke(message, result.RemoteEndPoint);
            }
            catch (Exception exc)
            {
                OnErrorThrown?.Invoke(exc);
            }
        }
    }

    public async Task SendAsync(Message message, IPEndPoint endPoint)
    {
        try
        {
            await _client.SendAsync(message.ToBytes(), endPoint, _cancellationToken);
            
        }
        catch (Exception exc)
        {
            OnErrorThrown?.Invoke(exc);
        }
    }

    public async Task CancelAsync()
    {
        await _cancellationTokenSource.CancelAsync();
    }


}
