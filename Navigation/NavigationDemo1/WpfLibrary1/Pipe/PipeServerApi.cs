using System.IO.Pipes;
using System.Windows;
using Common.Core.Enums;
using Common.Core.Messages;
using Common.Core.Pipe;

namespace CommunicationLibrary.Pipe;

public class PipeServerApi {
    private readonly PipeServer _server;
    private readonly MessageRouter _backgroundRouter = new(); // 后台路由
    private readonly MessageRouter _uiRouter = new();         // UI路由
    public event Action<PipeMessage>? MessageReceived;

    public PipeServerApi(string pipeName) {
        _server = new PipeServer(pipeName);
        _server.MessageReceived += msg => {
            MessageReceived?.Invoke(msg);
            _backgroundRouter.Dispatch(msg,false);
            if (Application.Current != null) {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    _uiRouter.Dispatch(msg,true);
                }));
            }
        };
    }
    /// <summary>注册后台路由（在后台线程执行）</summary>
    public void OnBackground(string command, Action<List<MessageParam>> handler) {
        _backgroundRouter.On(command, handler);
    }

    /// <summary>注册UI路由（自动切到UI线程执行）</summary>
    public void OnUi(string command, Action<List<MessageParam>> handler) {
        _uiRouter.On(command, handler);
    }

    /// <summary>
    /// 启动服务端
    /// </summary>
    public void Start() => _server.Start();

    /// <summary>
    /// 停止服务端
    /// </summary>
    public void Stop() => _server.Stop();

    /// <summary>
    /// 广播一条文本消息
    /// </summary>
    public void Broadcast(string command,params object[] args) {
        var msg = new PipeMessage() {
            Sender = ClientId.Server,
            Target = null,
            MessageType = MessageType.NORMAL,
            Timestamp = DateTime.Now
        };
        msg.Add(command);
        foreach (var arg in args)
            msg.Add(arg);
        _server.Broadcast(msg);
    }

   
    /// <summary>
    /// 给指定目标发送消息
    /// </summary>
    public void SendTo(ClientId target, string command,params object[] args) {
        var msg = new PipeMessage() {
            Sender = ClientId.Server,
            Target = target,
            MessageType = MessageType.NORMAL,
            Timestamp = DateTime.Now
        };
        msg.Add(command);
        foreach (var arg in args)
            msg.Add(arg);

        _server.SendToTarget(msg);
    }

    /// <summary>
    /// 获取当前在线的客户端列表
    /// </summary>
    public List<ClientId> GetOnlineClients() {
        // _server 里有 _clientMap，可以反射取
        var field = typeof(PipeServer).GetField("_clientMap",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field?.GetValue(_server) is Dictionary<ClientId, NamedPipeServerStream> dict) {
            return dict.Keys.ToList();
        }
        return new List<ClientId>();
    }

}