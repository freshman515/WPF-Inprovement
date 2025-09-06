using System.IO;
using System.IO.Pipes;
using System.Text;
using Common.Core.Enums;
using Common.Core.Messages;
using Newtonsoft.Json;

namespace Common.Core.Pipe;

public class PipeClient {
	private readonly string _pipeName;
	private NamedPipeClientStream? _client;

	public event Action<PipeMessage>? MessageReceived;
	public ClientId ClientName { get; }

	public PipeClient(string pipeName, ClientId clientName) {
		_pipeName = pipeName;
		ClientName = clientName;
	}
	public async Task ConnectAsync() {
		_client = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
		await _client.ConnectAsync();

		var register = new PipeMessage() {
			Sender = ClientName,
			MessageType = MessageType.REGISTER,
		};
		Send(register);
		_ = Task.Run(ReceiveLoop);
	}

	private async Task ReceiveLoop() {
		using var reader = new StreamReader(_client!,leaveOpen:true);
		while (_client!.IsConnected) {
			var line = await reader.ReadLineAsync();
			if (!string.IsNullOrWhiteSpace(line)) {
				try {
					var msg = JsonConvert.DeserializeObject<PipeMessage>(line);
					if (msg != null)
						MessageReceived?.Invoke(msg);
				} catch {
					Console.WriteLine($"[Client] 反序列化失败: {line}");
				}
			}
		}
	}
	public void Send(PipeMessage msg) {
		if (_client is not { IsConnected: true })
			return;
		msg.Sender = ClientName;
		var json = JsonConvert.SerializeObject(msg);
		var writer = new StreamWriter(_client, leaveOpen: true) {
			AutoFlush = true
		};
		writer.WriteLine(json);
	}
	public void Send(string command, params object[] args) {
		if (_client is not { IsConnected: true }) return;
		var msg = new PipeMessage {
			Sender = ClientName,
			MessageType = MessageType.NORMAL,
			Timestamp = DateTime.Now
		};
		msg.Add(command); // 参数0 = 命令字
		foreach (var arg in args)
			msg.Add(arg);

		Send(msg);

	}

}