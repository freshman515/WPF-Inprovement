using System.Net.Http;
using System.Net.Http.Json;
using Mall.Infrastructure.ExternalServices;
using WPFProject01.Dtos;
using WPFProject01.Helper;
using WPFProject01.Models;

namespace WPFProject01.Service;

public class ApiService : IApiService {
	private readonly HttpClient _httpClient;

	public ApiService() {
		_httpClient = new HttpClient() {
			BaseAddress = new Uri("https://localhost:7231")
		};
	}

	public async Task<List<Product>> GetProductsAsync() {
		var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<Product>>>("api/product");
		if (response is not null && response.Success) {
			return response.Data;
		}

		return new List<Product>();
	}

	public async Task UpdateProductAsync(Product product) {
		var id = product.Id;
		var dto = new UpdateProductDto() {
			Name = product.Name, Price = (decimal)product.Price, Description = product.Description,
			Stock = product.Stock
		};
		var response = await _httpClient.PutAsJsonAsync<UpdateProductDto>($"api/product/{id}", dto);
	}

	public async Task<WeatherResult> GetWeatherAsync(string city) {
		var res = await _httpClient.GetFromJsonAsync<ApiResponse<WeatherResult>>($"api/weather?city={city}");
		if (res.Success) {
			return res.Data;
		}
		return null;
	}
}