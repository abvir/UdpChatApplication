using System.Net;
using Core;

IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000);
ChatBase chat;
if(args.Length == 0){
    chat = new ChatServer(serverEndPoint);
}
else{
    chat = new ChatClient(serverEndPoint);
}
await chat.Run();