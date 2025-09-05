using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
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
        public IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            
            var services = new ServiceCollection();
            services.AddSingleton<IRegionNavigationService, RegionNavigationService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<EditViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ShellViewModel>();
            Services = services.BuildServiceProvider();
            var vm = new MainWindow() { DataContext = Services.GetRequiredService<MainViewModel>() };
            vm.Show();
        }
    }

}
