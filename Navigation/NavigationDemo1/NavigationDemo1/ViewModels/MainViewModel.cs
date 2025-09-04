using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NavigationDemo1.Interfaces;
using NavigationDemo1.Models;
using NavigationDemo1.Views;

namespace NavigationDemo1.ViewModels {
    public partial class MainViewModel :ObservableObject {
        [ObservableProperty] private FrameworkElement _currentView;
        [ObservableProperty] private int userId;
        [ObservableProperty] private string _name;
        private int count = 0;
        public INavigationService Nav { get; }

        public MainViewModel(INavigationService navigationService) {
            Nav = navigationService;
        }
        [RelayCommand]
        private void GoHome() {
            Nav.Go<HomeView,UserDetailArgs>(new UserDetailArgs(){UserId = UserId,UserName = Name});
        }
        [RelayCommand]
        private void GoEdit() {
            Nav.Go<EditView>();
        }
        [RelayCommand]
        private void GoSettings() {
            Nav.Go<SettingsView,string>($"helloworld{count++}");
        }
    }
}
