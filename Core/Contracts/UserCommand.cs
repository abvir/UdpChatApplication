using System.Text.Json;
using Entities;

namespace Contracts;

public class Contract{
    
    public string Command {get; protected set; } = string.Empty;
    internal static bool Is<T>(string text, out T? response)
    {
        response = JsonSerializer.Deserialize<T>(text);
        return response != null;
    }
    internal string Serialize() => JsonSerializer.Serialize(this);
}


public class JoinRequest : Contract
{
    public JoinRequest() => Command = "/register";
    public required string Username { get; set; }
}

public class JoinResponse : Contract
{
    public JoinResponse() => Command = "/register";
    public required User User { get; set; }
}

public class OnlineResponse : Contract
{
    public OnlineResponse() => Command = "/online";
    public required List<User> Users { get; set; }
}