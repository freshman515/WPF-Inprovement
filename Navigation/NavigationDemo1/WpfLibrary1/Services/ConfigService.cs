using System.IO;
using System.Text;
using System.Xml.Serialization;
using Common.Core.Interfaces;
using Newtonsoft.Json;

namespace Common.Core.Services {
	public class ConfigService :IConfigService{
		private readonly string _configRoot;

		public ConfigService(string? configRoot=null) { 
			_configRoot = configRoot ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
			if (!Directory.Exists(_configRoot)) {
				Directory.CreateDirectory(_configRoot);
			}
		}

		private string GetConfigPath(string fileName, string ext = ".json") {
			if (!fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
				fileName += ext;
			return Path.Combine(_configRoot, fileName);
		}

		#region JSON
		public async Task<T> LoadAsync<T>(string fileName) where T : class, new() {
			var path = GetConfigPath(fileName, ".json");
			if (!File.Exists(path)) {
				var def = new T();
				await SaveAsync(fileName, def);
				return def;
			}
			using var reader = new StreamReader(path, Encoding.UTF8);
			var json = await reader.ReadToEndAsync();
			return JsonConvert.DeserializeObject<T>(json) ?? new T();
		}

		public async Task SaveAsync<T>(string fileName, T config) where T : class, new() {
			var path = GetConfigPath(fileName, ".json");
			var json = JsonConvert.SerializeObject(config, Formatting.Indented);
			await File.WriteAllTextAsync(path, json, Encoding.UTF8);
		}

		public async Task UpdateAsync<T>(string fileName, Action<T> updater) where T : class, new() {
			var cfg = await LoadAsync<T>(fileName);
			updater(cfg);
			await SaveAsync(fileName, cfg);
		}
		#endregion

		#region XML
		public async Task<T> LoadXmlAsync<T>(string fileName) where T : class, new() {
			var path = GetConfigPath(fileName, ".xml");
			if (!File.Exists(path)) {
				var def = new T();
				await SaveXmlAsync(fileName, def);
				return def;
			}

			await using var stream = File.OpenRead(path);
			var serializer = new XmlSerializer(typeof(T));
			return serializer.Deserialize(stream) as T ?? new T();
		}

		public async Task SaveXmlAsync<T>(string fileName, T config) where T : class, new() {
			var path = GetConfigPath(fileName, ".xml");
			await using var stream = File.Create(path);
			var serializer = new XmlSerializer(typeof(T));
			serializer.Serialize(stream, config);
		}

		public async Task UpdateXmlAsync<T>(string fileName, Action<T> updater) where T : class, new() {
			var cfg = await LoadXmlAsync<T>(fileName);
			updater(cfg);
			await SaveXmlAsync(fileName, cfg);
		}
		#endregion
	}
}
