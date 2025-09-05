using CommunityToolkit.Mvvm.ComponentModel;
using RegeionNavigationDemo1.Interfaces;
using RegeionNavigationDemo1.Models;

namespace RegeionNavigationDemo1.ViewModels;

public partial class HomeViewModel :ObservableObject,INavigationReceiver {

    [ObservableProperty] private UserDetailArgs args;
    public void OnNavigated(object parameter) {
        if (parameter is UserDetailArgs args) {
            Args = args;
        }
    }
}