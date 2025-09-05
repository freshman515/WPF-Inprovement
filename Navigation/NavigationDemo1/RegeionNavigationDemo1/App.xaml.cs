using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using RegeionNavigationDemo1.Extension;
using RegeionNavigationDemo1.Interfaces;
using RegeionNavigationDemo1.Services;
using RegeionNavigationDemo1.ViewModels;
using RegeionNavigationDemo1.Views;
using SettingsViewModel = RegeionNavigationDemo1.ViewModels.SettingsViewModel;

namespace RegeionNavigationDemo1 {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public new static App Current => (App)Application.Current;
        public static IServiceProvider Services => Current._services;

        private IServiceProvider _services = null!;
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            var services = new ServiceCollection();
            services.AddSerivces();
            services.AddViewModels();
            _services = services.BuildServiceProvider();
            var main = _services.CreateView<MainWindow>();
            main.Show();
        }
    }

}
