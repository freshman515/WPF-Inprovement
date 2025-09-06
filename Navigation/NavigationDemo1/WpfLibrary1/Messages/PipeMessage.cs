using Common.Core.Enums;
using Common.Core.Pipe;
using CommunicationLibrary.Pipe;

namespace Common.Core.Messages;

public class PipeMessage {
    public ClientId Sender { get; set; }
    public ClientId? Target { get; set; } // null = 广播
    public List<MessageParam> Parameters { get; set; } = new();
    public MessageType MessageType { get; set; } = MessageType.NORMAL;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public void Add(object val) => Parameters.Add(new MessageParam(val));
    public void Add(object val, int index) => Parameters.Insert(index, new MessageParam(val));
}