using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using NavigationDemo1.Extensions;
using NavigationDemo1.Interfaces;
using NavigationDemo1.Service;
using NavigationDemo1.ViewModels;
using NavigationDemo1.Views;

namespace NavigationDemo1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _sp = null!;
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddViewModels();
            services.AddMiniNavigation<MainViewModel>();
            var sp = services.BuildServiceProvider();
            var vm = sp.GetRequiredService<MainViewModel>();
            var nav = sp.GetRequiredService<INavigationService>();
            nav.Go<HomeView>();

            var win = new MainWindow { DataContext = vm };
            win.Show();
        }

    }

}
