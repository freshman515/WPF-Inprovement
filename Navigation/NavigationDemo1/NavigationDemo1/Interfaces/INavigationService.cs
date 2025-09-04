using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NavigationDemo1.Interfaces {
    public interface INavigationService {
        void Go<TView>() where TView : FrameworkElement;
        void Go<TView,TParameter>(TParameter parameter) where TView : FrameworkElement;
        bool CanBack { get; }
        void Back();

    }
}
