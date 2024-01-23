using System.Net;
using Core.Chats;

IPEndPoint serverEndPoint = new(IPAddress.Parse("127.0.0.1"), 12000);
ChatBase chat;
if(args.Length == 0){
    chat = new ChatServer(hostEndPoint: serverEndPoint);
}
else{
    chat = new ChatClient(hostEndPoint: serverEndPoint, username: args[0]);
}
await chat.Run();