using System.Windows;
using Common.Core.Interfaces;

namespace Common.Core.Services;

public class RegionService :IRegionService{
    private readonly IServiceProvider? _provider;

    private readonly Dictionary<string, Action<FrameworkElement>> _regions = new();
    private readonly Dictionary<string, Stack<FrameworkElement>> _history = new();

    public RegionService(IServiceProvider? provider = null) {
        _provider = provider;
    }
    public void Register(string regionName, Action<FrameworkElement> setContent) {
        _regions[regionName] = setContent;
        if (!_history.ContainsKey(regionName))
            _history[regionName] = new Stack<FrameworkElement>();
    }
    public void Navigate<TView>(string regionName) where TView : FrameworkElement => InternalNavigate(regionName, typeof(TView), null, false);

    public void Navigate<TView, TParameter>(string regionName, TParameter parameter) where TView : FrameworkElement 
        =>InternalNavigate(regionName, typeof(TView), parameter, true);

    public bool CanBack(string regionName) => _history.ContainsKey(regionName) && _history[regionName].Count > 1;

    public void Back(string regionName) {
        if (!CanBack(regionName))
            return;
        var stack = _history[regionName];
        stack.Pop();
        var previous = stack.Peek();
        _regions[regionName](previous);
    }
    private void InternalNavigate(string regionName, Type viewType, object? parameter, bool hasParam) {
        if (!_regions.ContainsKey(regionName))
            throw new InvalidOperationException($"Region '{regionName}' not registered.");

        var view = (FrameworkElement)Activator.CreateInstance(viewType)!;

        var vmType = ResolveViewModelType(viewType);
        object? vm = null;
        if (vmType != null)
            vm = _provider?.GetService(vmType) ?? Activator.CreateInstance(vmType);

        view.DataContext = vm;

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