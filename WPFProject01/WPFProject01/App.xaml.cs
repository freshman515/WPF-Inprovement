using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WPFProject01.enums;
using WPFProject01.Messages;
using WPFProject01.Service;
using WPFProject01.ViewModels;
using WPFProject01.Views;

namespace WPFProject01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Service { get; private set; }

        public static async Task<bool> ShowMessageAsync(string title, string content, DialogButtons buttons)
        {
            var msg = new DialogMessage
            {
                Title = title,
                Content = content,
                Buttons = buttons
            };
            WeakReferenceMessenger.Default.Send(msg);
            var result = await msg.TaskSource.Task;
            return result == true;
        }


        public App()
        {
            Service = ConfigurationService();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Console.WriteLine("hello");
            //切换主题为白色
            // ThemeManager.Current.ChangeTheme(this, ThemeManager.Current.AddTheme(
            //     RuntimeThemeGenerator.Current.GenerateRuntimeTheme("Light", Colors.DarkSlateBlue)
            // ));

            //创建开始页面
            Service.GetService<ShellView>().Show();
        }

        private IServiceProvider ConfigurationService()
        {
            var service = new ServiceCollection();
            //依赖注入di容器
            var assembly = typeof(App).Assembly;
            //注入view和viewmodel
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract || type.Namespace == null ||
                    !type.Namespace.StartsWith("WPFProject01"))
                    continue;

                if (typeof(Window).IsAssignableFrom(type))
                {
                    service.AddSingleton(type);
                }

                if (typeof(UserControl).IsAssignableFrom(type))
                {
                    service.AddScoped(type);
                }

                if (type.Name.EndsWith("ViewModel") && !type.Name.EndsWith("UpdateViewModel"))
                {
                    service.AddScoped(type);
                }
                service.AddTransient<UpdateViewModel>();
                if (type.Name.EndsWith("Repository") || type.Name.EndsWith("Service"))
                {
                    var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == "I" + type.Name);
                    if (interfaceType != null)
                    {
                        service.AddScoped(interfaceType, type);
                    }
                }
            }

            //注册DbContext
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("Default");
            service.AddDbContext<MyDbContext>(options => { options.UseSqlServer(connectionString); });

            //注册导航
            // service.AddSingleton<INavigationService, NavigationService>();
            service.AddSingleton<INavigationService>(provider =>
                new NavigationService(provider, provider.GetService<ShellViewModel>()));
            //注册会话
            service.AddSingleton<UserSession>();


            return service.BuildServiceProvider();
        }
    }
}