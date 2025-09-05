using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RegeionNavigationDemo1.Interfaces;

namespace RegeionNavigationDemo1.Services;

public class RegionNavigationService :IRegionNavigationService{
    private readonly IServiceProvider? _provider;

    private readonly Dictionary<string, Action<FrameworkElement>> _regions = new();
    private readonly Dictionary<string, Stack<FrameworkElement>> _history = new();

    public RegionNavigationService(IServiceProvider? provider = null) {
        _provider = provider;
    }
    public void RegisterRegion(string regionName, Action<FrameworkElement> setContent) {
        _regions[regionName] = setContent;
        if (!_history.ContainsKey(regionName))
            _history[regionName] = new Stack<FrameworkElement>();
    }
    public void Go<TView>(string regionName) where TView : FrameworkElement => InternalGo(regionName, typeof(TView), null, false);

    public void Go<TView, TParameter>(string regionName, TParameter parameter) where TView : FrameworkElement 
        =>InternalGo(regionName, typeof(TView), parameter, true);

    public bool CanBack(string regionName) => _history.ContainsKey(regionName) && _history[regionName].Count > 1;

    public void Back(string regionName) {
        if (!CanBack(regionName))
            return;
        var stack = _history[regionName];
        stack.Pop();
        var previous = stack.Peek();
        _regions[regionName](previous);
    }
    private void InternalGo(string regionName, Type viewType, object? parameter, bool hasParam) {
        if (!_regions.ContainsKey(regionName))
            throw new InvalidOperationException($"Region '{regionName}' not registered.");

        var view = (FrameworkElement)Activator.CreateInstance(viewType)!;

        // 自动匹配 ViewModel
        var vmType = ResolveViewModelType(viewType);
        object? vm = null;
        if (vmType != null)
            vm = _provider?.GetService(vmType) ?? Activator.CreateInstance(vmType);

        view.DataContext = vm;

        // 参数传递
        if (hasParam && parameter != null && vm != null)
            InvokeReceiver(vm, parameter);

        _history[regionName].Push(view);
        _regions[regionName](view); // 更新 UI
    }

    private void InvokeReceiver(object? vm, object parameter) {
        if (vm == null)
            return;
        // 检查强类型接口
        foreach (var it in vm.GetType().GetInterfaces()) {
            if (!it.IsGenericType)
                continue;
            if (it.GetGenericTypeDefinition() != typeof(INavigationReceiver<>))
                continue;

            var tParam = it.GenericTypeArguments[0];
            if (!tParam.IsInstanceOfType(parameter))
                continue;

            var method = it.GetMethod(nameof(INavigationReceiver<object>.OnNavigated));
            method?.Invoke(vm, [parameter]);
            return;
        }

        // 检查弱类型接口
        if (vm is INavigationReceiver receiver)
            receiver.OnNavigated(parameter);
    }

    private static Type? ResolveViewModelType(Type viewType) {
        var name = viewType.FullName!;
        if (!name.EndsWith("View", StringComparison.Ordinal))
            return null;

        name = name.Replace(".Views.", ".ViewModels.");
        var vmName = name.Substring(0, name.Length - "View".Length) + "ViewModel";
        return viewType.Assembly.GetType(vmName);
    }
}