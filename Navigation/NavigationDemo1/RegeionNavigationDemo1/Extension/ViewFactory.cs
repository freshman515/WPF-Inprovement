using System.Windows;

namespace RegeionNavigationDemo1.Extension;

public static class ViewFactory {
    public static Window CreateWindowWithViewModel<TView>(IServiceProvider sp)
        where TView : Window, new() {
        var view = new TView();

        var vmType = ResolveViewModelType(typeof(TView));
        if (vmType != null) {
            var vm = sp.GetService(vmType) ?? Activator.CreateInstance(vmType);
            view.DataContext = vm;
        }
        return view;
    }

    private static Type? ResolveViewModelType(Type viewType) {
        var name = viewType.FullName!;
        if (!name.EndsWith("View"))
            return null;

        name = name.Replace(".Views.", ".ViewModels.");
        var vmName = name.Substring(0, name.Length - "View".Length) + "ViewModel";
        return viewType.Assembly.GetType(vmName);
    }
}
