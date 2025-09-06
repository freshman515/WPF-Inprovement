using Common.Core.Enums;
using Common.Core.Interfaces;
using Common.Core.Pipe;
using Microsoft.Extensions.DependencyInjection;
using NavigationDemo1.Extensions;
using NavigationDemo1.Interfaces;
using NavigationDemo1.Service;
using NavigationDemo1.ViewModels;
using NavigationDemo1.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace NavigationDemo1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; private set; } = null!;
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddViewModels();
            services.AddMiniNavigation<MainViewModel>();
            services.AddSingleton<IMessageClient>(sp => new PipeClientApi("MyPipe", ClientId.ClientM));
            var sp = services.BuildServiceProvider();
            Services = sp;
            var vm = sp.GetRequiredService<MainViewModel>();
            var nav = sp.GetRequiredService<INavigationService>();
            nav.Go<HomeView>();
            
            var win = new MainWindow { DataContext = vm };
            win.Show();
        }

    }

}
