using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Common.Core.Enums;
using Common.Core.Interfaces;
using Common.Core.Messages;
using Common.Core.Pipe;
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
        [ObservableProperty] private string _message2;
        [ObservableProperty] private string _message3;
        private int count = 0;
        private IMessageClient _client;
        public INavigationService Nav { get; }

        public MainViewModel(INavigationService navigationService,IMessageClient client) {
            _client = client;
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

        [RelayCommand]
        private async void StartClientAsync() {
            _client = new PipeClientApi("MyPipe",ClientId.ClientM);
            _client.OnUi("ShowMessage", args => Message3 = args[0].GetValue<string>());
            await _client.ConnectAsync();
        }
        [RelayCommand]
        private void SendMessage() {
            _client.SendTo(ClientId.ClientS, "ShowMessage", Message2);
        }
        [RelayCommand]
        private async void RequeseMessageAsync() {
            //_client.SendTo(ClientId.ClientS, "ShowMessage", Message2);
            var response = await _client.RequestAsync(ClientId.ClientS, "GetUserInfo", 123);
            var user = response.Parameters[0].GetValue<User>();
        }
    }
}
