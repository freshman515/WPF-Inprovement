using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NavigationDemo1.Interfaces;

namespace NavigationDemo1.Service {
    public class NavigationService:INavigationService {
        private readonly Action<FrameworkElement> _setContent;
        private readonly IServiceProvider? _provider;
        private readonly Stack<FrameworkElement> _history = new();
        public bool CanBack => _history.Count > 1;
        public NavigationService(Action<FrameworkElement> setContent, IServiceProvider? provider = null) {
            _setContent = setContent ?? throw new ArgumentNullException(nameof(setContent));
            _provider = provider;
        }
        public void Go<TView>() where TView : FrameworkElement 
            => InternalGo(typeof(TView),parameter:null,hasParam:false);

        public void Go<TView, TParameter>(TParameter parameter) where TView : FrameworkElement 
            => InternalGo(typeof(TView), parameter, hasParam: true);


        public void Back() {
            if (!CanBack)
                return;
            _history.Pop();
            _setContent(_history.Peek());
        }

        private void InternalGo(Type viewType, object? parameter, bool hasParam) {
            var view = (FrameworkElement)Activator.CreateInstance(viewType);
            var vmType = ResolveViewModelType(viewType);
            object? vm = null;
            if (vmType != null) {
                vm = _provider?.GetService(vmType) ?? Activator.CreateInstance(vmType);
            }

            view.DataContext = vm;
            if (hasParam && parameter != null && vm != null) {
                InvokeReceiver(vm, parameter);
            }
            _history.Push(view);
            _setContent(view);
        }

        private void InvokeReceiver(object? vm, object parameter) {
            if (vm == null)
                return;

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

            if (vm is INavigationReceiver receiver) {
                receiver.OnNavigated(parameter);
            }
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
}
