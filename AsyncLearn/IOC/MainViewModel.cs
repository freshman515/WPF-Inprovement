using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IOC.Services;
using MyLogging;
using MyServices;
using Serilog;

namespace IOC;

public partial class MainViewModel : ObservableObject {
	private readonly IMyLogger _logger;
	private readonly DeviceSerivce _deviceSerivce;
	private readonly ICatFactsService _catFactsService;
	private readonly IMessageBoxService _messageBoxService;
	[ObservableProperty] string title = "CatFacts";
	[ObservableProperty] private string limit;
	public ObservableCollection<string> Facts { get; set; } = new();
	public MainViewModel(IMyLogger logger, DeviceSerivce deviceSerivce,ICatFactsService catFactsService,IMessageBoxService messageBoxService) {
		_logger = logger;
		_deviceSerivce = deviceSerivce;
		_catFactsService = catFactsService;
		_messageBoxService = messageBoxService;
		logger.LogInformation("is Created!");
	}
	[RelayCommand]
	async Task GetFacts(string text) {
		if (int.TryParse(text, out var count)) {
					var facts = await _catFactsService.GetFactsAsync(count);
			foreach (var fact in facts) {
				Facts.Add(fact);
			}
		} else {
			_messageBoxService.ShowMessage("值不对");
		}
	}

	[RelayCommand]
	void CheckDevice() {
		_deviceSerivce.CheckDevice();
	}
}