using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NavigationDemo1.Interfaces;
using NavigationDemo1.Models;

namespace NavigationDemo1.ViewModels;

public partial class HomeViewModel :ObservableObject,INavigationReceiver {
    private readonly INavigationService _nav;

    [ObservableProperty] private UserDetailArgs args;
    public HomeViewModel(INavigationService navigationService) {
        _nav = navigationService;
    }
    public void OnNavigated(object parameter) {
        if (parameter is UserDetailArgs args) {
            Args = args;
        }
    }
}