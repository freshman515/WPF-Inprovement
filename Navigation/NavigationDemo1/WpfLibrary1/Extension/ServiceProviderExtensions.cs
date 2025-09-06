using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows;

namespace Common.Core.Extension;

public static class ServiceProviderExtensions {
    public static TView CreateView<TView>(this IServiceProvider sp) where TView : FrameworkElement,new() {
        var view = new TView();
        var vmType = ResolveViewModelType(typeof(TView));
        if (vmType != null) {
            var vm = sp.GetService(vmType)?? ActivatorUtilities.CreateInstance(sp, vmType);
            view.DataContext = vm;
        }
        return view;
    }
    private static Type? ResolveViewModelType(Type viewType) {
        var fullName = viewType.FullName;
        if (string.IsNullOrEmpty(fullName))
            return null;

        string ns = viewType.Namespace ?? "";
        string typeName = viewType.Name;
        string vmName;

        if (typeName.EndsWith("View", StringComparison.Ordinal)) {
            vmName = typeName.Substring(0, typeName.Length - "View".Length) + "ViewModel";
        } else if (typeName.EndsWith("Window", StringComparison.Ordinal)) {
            vmName = typeName.Substring(0, typeName.Length - "Window".Length) + "ViewModel";
        } else if (typeName.EndsWith("Dialog", StringComparison.Ordinal)) {
            vmName = typeName + "ViewModel";
        } else {
            return null;
        }

        if (ns.Contains(".Views"))
            ns = ns.Replace(".Views", ".ViewModels");
        else if (!string.IsNullOrEmpty(ns))
            ns = ns + ".ViewModels"; 
        var finalName = string.IsNullOrEmpty(ns) ? vmName : ns + "." + vmName;
        return viewType.Assembly.GetType(finalName);
    }


    /// <summary>
    /// 批量注册指定程序集下的所有 *ViewModel
    /// </summary>
    public static IServiceCollection AddViewModelsFromAssembly(this IServiceCollection services, Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Transient) {
        var vmTypes = assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel") )
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
    /// <summary>
    /// 批量注册指定库（比如 RegeionNavigationDemo1 或其它 UI 模块）的 ViewModel
    /// </summary>
    public static IServiceCollection AddViewModelsFrom<T>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient) {
        return services.AddViewModelsFromAssembly(typeof(T).Assembly, lifetime);
    }
    /// <summary>
    /// 批量注册接口和实现：
    /// 如果有接口 → 注册接口和实现
    /// 如果没有接口 → 注册自己
    /// </summary>
    public static IServiceCollection AddServicesFromAssembly(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Singleton) {
        var types = assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.Name.Contains("<"))  
            .ToList();
        foreach (var implType in types) {
            var interfaceType = implType.GetInterfaces().FirstOrDefault();
            if (interfaceType != null) {
                services.Add(new ServiceDescriptor(interfaceType, implType, lifetime));
            } else {
                services.Add(new ServiceDescriptor(implType, implType, lifetime));
            }
        }
        services.AddSingleton<IMessenger>(sp => WeakReferenceMessenger.Default);
        return services;
    }

    public static IServiceCollection AddSerivces(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton) {
        return services.AddServicesFromAssembly(Assembly.GetExecutingAssembly(), lifetime);
    }
    public static IServiceCollection AddSerivcesFrom<T>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton) {
        return services.AddServicesFromAssembly(typeof(T).Assembly, lifetime);
    }
    public static IServiceCollection AddCommonCoreServices(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton) {
        return services.AddServicesFromAssembly(typeof(Interfaces.IMessageServer).Assembly, lifetime);
    }

}