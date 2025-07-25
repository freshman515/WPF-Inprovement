using Mall.Infrastructure.ExternalServices;
using WPFProject01.Models;

namespace WPFProject01.Service;

public interface IApiService {
	Task<List<Product>> GetProductsAsync();
	Task UpdateProductAsync(Product product);
	Task<WeatherResult> GetWeatherAsync(string city);
}