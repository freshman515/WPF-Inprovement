using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Xml.Linq;
using RegeionNavigationDemo1.Interfaces;
using RegeionNavigationDemo1.Models;
using RegeionNavigationDemo1.Views;

namespace RegeionNavigationDemo1.ViewModels;

public partial class ShellViewModel : ObservableObject {
    private readonly IRegionNavigationService _nav;
    [ObservableProperty] private string message = "hello";
    [ObservableProperty] private FrameworkElement currentView;

    public ShellViewModel(IRegionNavigationService regionNavigationService) {
        _nav = regionNavigationService;
        _nav.RegisterRegion("ShellRegion", v => CurrentView = v);
    }
    [RelayCommand]
    private void GoHome() {
        _nav.Go<HomeView, UserDetailArgs>("ShellRegion", new UserDetailArgs() { UserId = 123, UserName = "dsfs" });
    }
    [RelayCommand]
    private void GoEdit() {
        _nav.Go<EditView>("ShellRegion");
    }
    int count=0;

    [RelayCommand]
    private void GoSettings() {
        _nav.Go<SettingsView, string>("MainRegion", $"helloworld{count++}");
    }
}