using System.Text;
using System.Text.Json;
using Entities;

namespace Core.Extensions;

public static class MessageExtensions
{
    public static byte[] ToBytes(this Message message)
    => Encoding.UTF8.GetBytes(JsonSerializer.Serialize<Message>(message));

    public static Message? ToMessage(this byte[] bytes)
        => JsonSerializer.Deserialize<Message>(Encoding.UTF8.GetString(bytes));


}