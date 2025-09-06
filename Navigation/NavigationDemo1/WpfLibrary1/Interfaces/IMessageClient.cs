using Common.Core.Enums;
using Common.Core.Messages;
using Common.Core.Pipe;

namespace Common.Core.Interfaces;

public interface IMessageClient {
    event Action<PipeMessage>? MessageReceived;
    ClientId ClientId { get; }
    Task ConnectAsync();
    void Broadcast(string command, params object[] args);
    void SendTo(ClientId target, string command, params object[] args);
    void OnBackground(string command, Action<List<MessageParam>> handler);
    void OnUi(string command, Action<List<MessageParam>> handler);
}