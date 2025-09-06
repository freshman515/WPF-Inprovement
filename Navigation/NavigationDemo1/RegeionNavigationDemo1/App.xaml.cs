using Common.Core.Extension;
using Common.Core.Interfaces;
using Common.Core.Pipe;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Enrichers.CallerInfo;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Windows;
using Common.Core.Enums;
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
            services.AddCommonCoreServices();
            services.AddSingleton<IMessageClient>(sp => new PipeClientApi("MyPipe",ClientId.ClientS));
            services.AddViewModels();
            _services = services.BuildServiceProvider();
            InitLog();
            var main = _services.CreateView<MainWindow>();
            main.Show();
        }

        private static void InitLog() {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithCallerInfo(
                    includeFileInfo: true,
                    assemblyPrefix: assemblyName,
                    filePathDepth: 3
                ) 
                .WriteTo.Debug(
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] " +
                    "({SourceFile}:{LineNumber}) {Message:lj}{NewLine}{Exception}"
                )
                .WriteTo.File(
                    path: "logs/app.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    encoding: System.Text.Encoding.UTF8,
                    outputTemplate:
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] " +
                    "({SourceFile}:{LineNumber}) {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();
        }
    }

}
