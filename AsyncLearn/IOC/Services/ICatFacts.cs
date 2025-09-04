namespace IOC.Services;

public interface ICatFactsService {
	Task<IEnumerable<string>> GetFactsAsync(int count);
}