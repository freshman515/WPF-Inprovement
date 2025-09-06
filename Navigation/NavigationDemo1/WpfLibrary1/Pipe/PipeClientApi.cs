using System.Windows;
using Com.Pipe;
using Common.Core.Enums;
using Common.Core.Pipe;
using Common.Core.Enums;
using Common.Core.Interfaces;
using Common.Core.Messages;
using CommunicationLibrary.Pipe;

namespace Common.Core.Pipe;

public class PipeClientApi: IMessageClient {
    private readonly PipeClient _client;
    private readonly MessageRouter _backgroundRouter = new(); // 后台线程路由
    private readonly MessageRouter _uiRouter = new();         // UI线程路由

    public event Action<PipeMessage>? MessageReceived;
    public ClientId ClientId => _client.ClientName;

    public PipeClientApi(string pipeName, ClientId clientId) {
        _client = new PipeClient(pipeName, clientId);
        _client.MessageReceived += msg => {

            MessageReceived?.Invoke(msg);
            _backgroundRouter.Dispatch(msg,false);
            if (Application.Current != null) {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    _uiRouter.Dispatch(msg,true);
                }));
            }
        };
    }

    /// <summary>
    /// 注册后台路由（在后台线程执行）
    /// </summary>
    public void OnBackground(string command, Action<List<MessageParam>> handler) {
        _backgroundRouter.On(command, handler);
    }

    /// <summary>
    /// 注册UI路由（自动切到UI线程执行）
    /// </summary>
    public void OnUi(string command, Action<List<MessageParam>> handler) {
        _uiRouter.On(command, handler);
    }

    /// <summary>
    /// 连接到服务端
    /// </summary>
    public async Task ConnectAsync() {
        await _client.ConnectAsync();
    }

    /// <summary>
    /// 广播（发给所有其他客户端）
    /// </summary>
    public void Broadcast(string command, params object[] args) {
        var msg = new PipeMessage {
            Sender = ClientId,
            Target = null,
            MessageType = MessageType.NORMAL,
            Timestamp = DateTime.Now
        };

        msg.Add(command); 
        foreach (var arg in args)
            msg.Add(arg);

        _client.Send(msg);
    }
    /// <summary>
    /// 点对点发送
    /// </summary>
    

    public void SendTo(ClientId target, string command, params object[] args) {
        var msg = new PipeMessage {
            Sender = ClientId,
            Target = target,
            MessageType = MessageType.NORMAL,
            Timestamp = DateTime.Now
        };
        msg.Add(command); // 参数0 = 命令字
        foreach (var arg in args)
            msg.Add(arg);
        _client.Send(msg);


    }



}