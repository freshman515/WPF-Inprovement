using Common.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using RegeionNavigationDemo1.Models;

namespace RegeionNavigationDemo1.ViewModels;

public partial class HomeViewModel :ObservableObject,INavigationReceiver {
    private readonly IMessenger _messenger;


    [ObservableProperty] private UserDetailArgs args;

    public HomeViewModel(IMessenger messenger) {
        _messenger = messenger;
        _messenger.Register<UserDetailArgs, string>(this, "Home", (_,m)=>  Args=m);
    }

    public void OnNavigated(object parameter) {
        if (parameter is UserDetailArgs args) {
            Args = args;
        }
    }
}