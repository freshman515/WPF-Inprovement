using System.Reflection;
using Common.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NavigationDemo1.Interfaces;
using NavigationDemo1.Service;

namespace NavigationDemo1.Extensions {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddMiniNavigation<TShellViewModel>(
            this IServiceCollection services)
            where TShellViewModel : class {
            TShellViewModel? shellVm = null;

            services.AddSingleton<INavigationService>(sp => {
                return new NavigationService(v => {
                    var prop = typeof(TShellViewModel).GetProperty("CurrentView");
                    prop?.SetValue(shellVm, v);
                }, sp);
            });
            services.AddSingleton<TShellViewModel>(sp => {
                var nav = sp.GetRequiredService<INavigationService>();
                var cli = sp.GetRequiredService<IMessageClient>();
                shellVm = ActivatorUtilities.CreateInstance<TShellViewModel>(sp, nav,cli);
                return shellVm;
            });

            return services;
        }
        /// <summary>
        /// 批量注册指定程序集下的所有 *ViewModel
        /// </summary>
        public static IServiceCollection AddViewModelsFromAssembly(this IServiceCollection services, Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Transient) {
            var vmTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel") && !t.Name.EndsWith("MainViewModel"))
                .ToList();
            foreach (var vmType in vmTypes) {
                services.Add(new ServiceDescriptor(vmType, vmType, lifetime));
            }
            return services;
        }
        /// <summary>
        /// 批量注册当前执行程序集的 ViewModel
        /// </summary>
        public static IServiceCollection AddViewModels(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Transient) {
            return services.AddViewModelsFromAssembly(Assembly.GetExecutingAssembly(), lifetime);
        }
    }
}
