using System.Windows;

namespace Common.Core.Interfaces;

public interface IRegionService {
    void Register(string regionName, Action<FrameworkElement> setContent);
    void Navigate<TView>(string regionName) where TView : FrameworkElement;
    void Navigate<TView, TParameter>(string regionName, TParameter parameter) where TView : FrameworkElement;
    bool CanBack(string regionName);
    void Back(string regionName);
}