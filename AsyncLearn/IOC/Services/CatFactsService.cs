using Newtonsoft.Json.Linq;

namespace IOC.Services;

public class CatFactsService :ICatFactsService{
	private readonly IWebClient _webClient;

	public CatFactsService(IWebClient webClient) {
		_webClient = webClient;
	}
	public async Task<IEnumerable<string>> GetFactsAsync(int count) {
		var url = $"https://catfact.ninja/facts?limit={count}";
		var response = await _webClient.GetStringAsync(url);
		var data = JObject.Parse(response);
		return data["data"].Select(x => x["fact"].ToString());
	}
}