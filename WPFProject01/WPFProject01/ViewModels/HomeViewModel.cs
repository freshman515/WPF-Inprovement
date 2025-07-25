using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mall.Infrastructure.ExternalServices;
using NPinyin;
using WPFProject01.Helper;
using WPFProject01.Models;
using WPFProject01.Service;

namespace WPFProject01.ViewModels;

public partial class HomeViewModel : ObservableObject {
	private readonly IApiService _apiService;
	public UserSession UserSession { get; }

	[ObservableProperty] private ObservableCollection<Product> _products = new();
	private Product _selectedProduct;

	[ObservableProperty] private WeatherResult _weather;
	[ObservableProperty] private string _city = "重庆";

	private readonly Dictionary<string, string> cityMap = new() {
		{ "成都", "chengdu" },
		// 直辖市
		{ "北京", "beijing" },
		{ "天津", "tianjin" },
		{ "上海", "shanghai" },
		{ "重庆", "chongqing" },

		// 省会城市
		{ "广州", "guangzhou" },
		{ "深圳", "shenzhen" },
		{ "武汉", "wuhan" },
		{ "南京", "nanjing" },
		{ "杭州", "hangzhou" },
		{ "长沙", "changsha" },
		{ "郑州", "zhengzhou" },
		{ "西安", "xian" },
		{ "合肥", "hefei" },
		{ "沈阳", "shenyang" },
		{ "青岛", "qingdao" },
		{ "大连", "dalian" },
		{ "哈尔滨", "haerbin" },
		{ "济南", "jinan" },
		{ "福州", "fuzhou" },
		{ "昆明", "kunming" },
		{ "南昌", "nanchang" },
		{ "贵阳", "guiyang" },
		{ "兰州", "lanzhou" },
		{ "呼和浩特", "huhehaote" },
		{ "南宁", "nanning" },
		{ "乌鲁木齐", "wulumuqi" },

		// 其他重要城市
		{ "苏州", "suzhou" },
		{ "无锡", "wuxi" },
		{ "常州", "changzhou" },
		{ "泉州", "quanzhou" },
		{ "石家庄", "shijiazhuang" },
	};

	public HomeViewModel(IApiService apiService, UserSession userSession) {
		_apiService = apiService;
		UserSession = userSession;
		Products.CollectionChanged += Products_CollectionChanged;
		_ = GetWeather();
		_ = Refresh();
	}

	[RelayCommand]
	async Task GetCityWeather() {
		await GetWeather();
	}

	private async Task GetWeather() {
		try {
			var code = cityMap[City];
			Weather = await _apiService.GetWeatherAsync(code);
		} catch (Exception e) {
			Console.WriteLine("找不到该城市");
			GlobalSnackbar.Show("找不到该城市");
		}
	}


	private void Products_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
		// 如果有新加的 Product
		if (e.NewItems != null) {
			foreach (Product item in e.NewItems) {
				item.PropertyChanged += Product_PropertyChanged;
			}
		}

		// 如果有删掉的 Product
		if (e.OldItems != null) {
			foreach (Product item in e.OldItems) {
				item.PropertyChanged -= Product_PropertyChanged;
			}
		}
	}

	private async void Product_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
		var product = sender as Product;
		if (product == null) {
			return;
		}

		await _apiService.UpdateProductAsync(product);
	}

	private string GetCode(string cityChinese) {
		var pinyin = Pinyin.GetPinyin(cityChinese);
		return pinyin.ToLower();
	}

	[RelayCommand]
	async Task Refresh() {
		try {
			var result = await _apiService.GetProductsAsync();
			Products.Clear(); // 清空原集合（保留监听）
			foreach (var product in result) {
				Products.Add(product); // 会触发 CollectionChanged，每个 item 会注册 PropertyChanged
			}

			await GetWeather();
		} catch (Exception e) {
			GlobalSnackbar.Show(e.Message);
		}
	}
}