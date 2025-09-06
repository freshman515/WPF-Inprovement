using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Xml.Linq;
using Common.Core.Enums;
using Common.Core.Interfaces;
using Common.Core.Messages;
using Common.Core.Pipe;
using RegeionNavigationDemo1.Models;
using RegeionNavigationDemo1.Views;

namespace RegeionNavigationDemo1.ViewModels;

public partial class ShellViewModel : ObservableObject {
    private readonly IRegionService _nav;
    private IMessageClient _client;
    [ObservableProperty] private string message = "hello";
    [ObservableProperty] private FrameworkElement currentView;
    [ObservableProperty] private string _message2;
    [ObservableProperty] private string _message3;
    [ObservableProperty] private bool _message4;
    [ObservableProperty] private string _message5;
    public ShellViewModel(IRegionService regionService,IMessageClient client) {
        _client = client;
        _nav = regionService;
        _nav.Register("ShellRegion", v => CurrentView = v);
    }
    [RelayCommand]
    private void GoHome() {
        _nav.Navigate<HomeView, UserDetailArgs>("ShellRegion", new UserDetailArgs() { UserId = 123, UserName = "dsfs" });
    }
    [RelayCommand]
    private void GoEdit() {
        _nav.Navigate<EditView>("ShellRegion");
    }
    int count=0;

    [RelayCommand]
    private void GoSettings() {
        _nav.Navigate<SettingsView, string>("MainRegion", $"helloworld{count++}");
    }

    [RelayCommand]
    private async void StartClientAsync() {
        await _client.ConnectAsync();

        _client.OnUi("ShowMessage", args => {
            Message3 = args[0].GetValue<string>();
        });

        _client.OnUi("IsEnglish" ,args=> Message4 = args[0].GetValue<bool>());

        _client.OnBackground("GetUserInfo", args => {
            var id = args[0].GetValue<int>();
            var user = new User { Id = id, Name = "Alice" };
            return new object[] { user };
        });
    }

    [RelayCommand]
    private void SendMessage() {
        _client.SendTo(ClientId.ClientM,"ShowMessage",Message2);
    }
    [RelayCommand]
    private void SendMessageToServer() {
        _client.SendTo(ClientId.Server, "ShowMessage", Message5);
    }
}