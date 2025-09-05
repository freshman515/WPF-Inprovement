using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RegeionNavigationDemo1.Interfaces;

public interface IRegionNavigationService {
    void RegisterRegion(string regionName, Action<FrameworkElement> setContent);
    void Go<TView>(string regionName) where TView : FrameworkElement;
    void Go<TView, TParameter>(string regionName, TParameter parameter) where TView : FrameworkElement;
    bool CanBack(string regionName);
    void Back(string regionName);
}