using System.Net.Http;

namespace IOC.Services;

public class WebClient : IWebClient {
	private HttpClient _client = new HttpClient();
	public async Task<string> GetStringAsync(string url) {
		return await _client.GetStringAsync(url);
	}
}