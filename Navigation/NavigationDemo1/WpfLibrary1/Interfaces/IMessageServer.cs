using Common.Core.Enums;
using Common.Core.Messages;
using Common.Core.Pipe;

namespace Common.Core.Interfaces;

public interface IMessageServer {
    event Action<PipeMessage>? MessageReceived;
    void Start();
    void Stop();
    void Broadcast(string command, params object[] args);
    void SendTo(ClientId target, string command, params object[] args);
    List<ClientId> GetOnlineClients();
    void OnBackground(string command, Action<List<MessageParam>> handler);
    void OnUi(string command, Action<List<MessageParam>> handler);
    Task<PipeMessage> RequestAsync(ClientId target, string command, params object[] args);
    void Respond(PipeMessage request, params object[] results);
}