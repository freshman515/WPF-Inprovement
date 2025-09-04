namespace IOC.Services;

public interface IWebClient {
	Task<string> GetStringAsync(string url);
}