using System.IO;
using System.IO.Pipes;
using System.Text;
using Common.Core.Enums;
using Common.Core.Enums;
using Common.Core.Messages;
using Newtonsoft.Json;

namespace Common.Core.Pipe {
	public class PipeServer {
		public event Action<PipeMessage>? MessageReceived; // 收到消息的事件
		private readonly string _pipeName;
		private readonly List<NamedPipeServerStream> _clients = new();
		private readonly CancellationTokenSource _cts = new();
		private readonly Dictionary<ClientId, NamedPipeServerStream> _clientMap = new();
		public PipeServer(string pipeName) {
			_pipeName = pipeName;
		}

		public void Start() {
			Console.WriteLine($"[Server] Started. Pipe = {_pipeName}");
			_ = Task.Run(AcceptLoop);
		}

		private async Task AcceptLoop() {
			while (!_cts.IsCancellationRequested) {
				var serverStream = new NamedPipeServerStream(
					_pipeName,
					PipeDirection.InOut,
					NamedPipeServerStream.MaxAllowedServerInstances,
					PipeTransmissionMode.Message,
					PipeOptions.Asynchronous);

				await serverStream.WaitForConnectionAsync(_cts.Token);
				lock (_clients)
					_clients.Add(serverStream);
				_ = Task.Run(() => HandleClientAsync(serverStream));
			}
		}
		private async Task HandleClientAsync(NamedPipeServerStream client) {
			using var reader = new StreamReader(client, Encoding.UTF8);
			while (client.IsConnected && !_cts.IsCancellationRequested) {
				var line = await reader.ReadLineAsync();
				if (line == null) return;

				try {
					var msg = JsonConvert.DeserializeObject<PipeMessage>(line);
					if (msg == null) continue;

					if (msg.MessageType == MessageType.REGISTER) {
						_clientMap[msg.Sender] = client;
						MessageReceived.Invoke(msg);
						continue;
					}
					if (msg.Target==null)
						Broadcast(msg, client);
					else {
						if (msg.Target == ClientId.Server) 
							MessageReceived?.Invoke(msg);
						else 
							SendToTarget(msg);       
					}
				} catch {
					Console.WriteLine($"[Server] 反序列化失败: {line}");
				}
			}

			Console.WriteLine("[Server] Client disconnected.");
			lock (_clients)
				_clients.Remove(client);
		}

		public void SendToTarget(PipeMessage msg) {
			if (msg.Target == null) return;
			if (_clientMap.TryGetValue(msg.Target.Value, out var targetClient) && targetClient.IsConnected) {
				var json = JsonConvert.SerializeObject(msg);
				var writer = new StreamWriter(targetClient,leaveOpen: true) {
					AutoFlush = true
				};
				writer.WriteLine(json);
			}
			
		}

		private void Broadcast(PipeMessage message, NamedPipeServerStream fromClient) {
			var json = JsonConvert.SerializeObject(message);
			lock (_clients) {
				foreach (var client in _clients) {
					if (client.IsConnected && client != fromClient) {
						var writer = new StreamWriter(client, leaveOpen: true) {
							AutoFlush = true
						};
						writer.WriteLine(json);
					}
				}
			}
		}

		public void Broadcast(PipeMessage message) {
			var json = JsonConvert.SerializeObject(message);
			lock (_clients) {
				foreach (var cli in _clients) {
					var writer = new StreamWriter(cli, leaveOpen: true) {
						AutoFlush = true
					};
					writer.WriteLine(json);
				}
			}
		}

		public void Stop() {
			_cts.Cancel();
			lock (_clients) {
				foreach (var client in _clients)
					client.Dispose();
				_clients.Clear();
			}
			Console.WriteLine("[Server] Stopped.");
		}
	}
}
