using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RegeionNavigationDemo1.Interfaces;
using RegeionNavigationDemo1.Models;
using RegeionNavigationDemo1.Views;
using RegeionNavigationDemo1;
using RegeionNavigationDemo1.Extension;

namespace RegeionNavigationDemo1.ViewModels {
    public partial class MainViewModel : ObservableObject {
        [ObservableProperty] private FrameworkElement _currentView;
        [ObservableProperty] private FrameworkElement _currentView2;
        [ObservableProperty] private int userId;
        [ObservableProperty] private string _name;
        private int count = 0;
        private readonly IRegionNavigationService _nav;

        public MainViewModel(IRegionNavigationService navigationService) {
            _nav = navigationService;
            _nav.RegisterRegion("MainRegion", v => CurrentView = v);
            _nav.RegisterRegion("OtherRegion", v => CurrentView2 = v);
        }

        [RelayCommand]
        private void ShowNewWindiow() {
           var view =  ViewFactory.CreateWindowWithViewModel<ShellView>(App.Current.Services);
           view.Show();
        }
        [RelayCommand]
        private void GoHome() {
            _nav.Go<HomeView, UserDetailArgs>("MainRegion", new UserDetailArgs() { UserId = UserId, UserName = Name });
        }
        [RelayCommand]
        private void GoEdit() {
            _nav.Go<EditView>("MainRegion");
        }
        [RelayCommand]
        private void GoSettings() {
            _nav.Go<SettingsView, string>("MainRegion", $"helloworld{count++}");
        }

        [RelayCommand]
        private void GoHomeRegion2() {
            _nav.Go<HomeView, UserDetailArgs>("OtherRegion", new UserDetailArgs() { UserId = UserId, UserName = Name });
        }
        [RelayCommand]
        private void GoEditRegion2() {
            _nav.Go<EditView>("OtherRegion");
        }
        [RelayCommand]
        private void GoSettingsRegion2() {
            _nav.Go<SettingsView, string>("OtherRegion", $"nihaoa{count+=100}");
        }
    }
}
