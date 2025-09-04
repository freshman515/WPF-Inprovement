using System.Configuration;
using System.Data;
using System.Windows;
using IOC.Services;
using Microsoft.Extensions.DependencyInjection;
using MyLogging;
using MyServices;
using Serilog;

namespace IOC;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
	public App() {
		Services = ConfigureSerivces();
	}

	

	public new static App Current => (App)Application.Current;
	public IServiceProvider Services { get; }

	private static IServiceProvider ConfigureSerivces() {
		var services = new ServiceCollection();
		//services.AddSingleton<ILogger>(_=>new LoggerConfiguration()
		//	.MinimumLevel.Debug().WriteTo.File("log.txt").CreateLogger()
		//);
		services.AddSingleton<IWebClient, WebClient>();
		
		services.AddMyLogging();
		//services.AddMyServices();
		//services.AddSingleton<ICatFactsService, CatFactsService>();
		//services.AddSingleton<IMessageBoxService, MessageBoxService>();
		services.AddMyServices();
		services.AddTransient<MainViewModel>();
		services.AddTransient(sp=>new MainWindow(){DataContext = sp.GetService<MainViewModel>()});


		return services.BuildServiceProvider();
	}

	private void App_OnStartup(object sender, StartupEventArgs e) {
		Services.GetService<MainWindow>()?.Show();
	}
}