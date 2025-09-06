using System.Configuration;
using System.Data;
using System.Windows;
using Common.Core.Extension;
using Common.Core.Interfaces;
using Common.Core.Pipe;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using ServerPipe.ViewModels;

namespace ServerPipe {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public static IServiceProvider Serivces = Currnet._services;
		public new static App Currnet = (App)Application.Current;
		private ServiceProvider _services;

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);
			var services = new ServiceCollection();
			services.AddCommonCoreServices();
			services.AddSingleton<IMessageServer>(sp => new PipeServerApi("MyPipe"));
			services.AddViewModelsFrom<MainViewModel>();
			_services = services.BuildServiceProvider();
			var main = _services.CreateView<MainWindow>();
			main.Show();
		}
	}

}
