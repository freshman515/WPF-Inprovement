using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RegeionNavigationDemo1.Models;
using RegeionNavigationDemo1.Views;
using System.Windows;
using Common.Core.Extension;
using Common.Core.Interfaces;
using CommunityToolkit.Mvvm.Messaging;
using Serilog;

namespace RegeionNavigationDemo1.ViewModels {
    public partial class MainViewModel : ObservableObject {
        [ObservableProperty] private FrameworkElement? _currentView;
        [ObservableProperty] private FrameworkElement? _currentView2;
        [ObservableProperty] private int userId;
        [ObservableProperty] private string? _name;
        [ObservableProperty] private List<Channel> _channels = [
            new() { Id = 1, Name = "channel1", Type = "压力" }, 
            new() { Id = 2, Name = "channel2", Type = "速度" },
            new() { Id = 3, Name = "channel3", Type = "流量" }
        ];
        private int count = 0;
        private readonly IRegionService _region;
        private readonly IDialogSerivce _dialogService;
        private readonly INotificationService _notificationService;
        private readonly IConfigService _configService;
        private readonly IMessenger _messenger;

        public MainViewModel(IRegionService region,IDialogSerivce dialogService,INotificationService notificationService,IConfigService configService,IMessenger messenger) {
            _region = region;
            _dialogService = dialogService;
            _notificationService = notificationService;
            _configService = configService;
            _messenger = messenger;
            _region.Register("MainRegion", v => CurrentView = v);
            _region.Register("OtherRegion", v => CurrentView2 = v);
        }

        [RelayCommand]
        private void ShowNewWindiow() {
            var view = App.Services.CreateView<ShellView>();
            view.Show();
        }
        [RelayCommand]
        private void GoHome() {
            _region.Navigate<HomeView, UserDetailArgs>("MainRegion", new UserDetailArgs() { UserId = UserId, UserName = Name });
            _notificationService.ShowSuccess("回到HomeView");
            Log.Debug("回到HomeView");
            Log.Warning("回到HomeView");
            Log.Information("回到HomeView");
            Log.Error("回到HomeView");
        }
        [RelayCommand]
        private void GoEdit() {
            _region.Navigate<EditView>("MainRegion");
            _notificationService.ShowHint("导航到EditView");
            Log.Debug("导航到EditView");
            Log.Warning("导航到EditView");
            Log.Information("导航到EditView");
            Log.Error("导航到EditView");
        }
        [RelayCommand]
        private void GoSettings() {
            _region.Navigate<SettingsView, string>("MainRegion", $"helloworld{count++}");
            _notificationService.ShowError("导航到SettingsView");
            Log.Debug("导航到SettingsView");
            Log.Warning("导航到SettingsView");
            Log.Information("导航到SettingsView");
            Log.Error("导航到SettingsView");

        }

        [RelayCommand]
        private void GoHomeRegion2() {
            _region.Navigate<HomeView, UserDetailArgs>("OtherRegion", new UserDetailArgs() { UserId = UserId, UserName = Name });
        }
        [RelayCommand]
        private void GoEditRegion2() {
            _region.Navigate<EditView>("OtherRegion");
        }
        [RelayCommand]
        private void GoSettingsRegion2() {
            _region.Navigate<SettingsView, string>("OtherRegion", $"nihaoa{count+=100}");
        }

        [RelayCommand]
        private async void DeleteItemAsync() {
            var result = await _dialogService.ShowDialogAsync<ConfirmDialog, string, bool>("确定要删除吗");
            _dialogService.ShowMessageBox(result?"删除成功":"删除失败");
        }

        [RelayCommand]
        private async Task SaveChannelsAsync() {
           await _configService.SaveAsync("channel", Channels);
            Log.Information("保存Channels成功");
        }
        [RelayCommand]
        private async Task LoadChannelsAsync() {
            var channels = await _configService.LoadXmlAsync<List<Channel>>("channel");
            Log.Information("导出Channels成功");
        }
        [RelayCommand]
        private  void SendMessageToHome() {
            _messenger.Send(new UserDetailArgs() { UserId = 20, UserName = "好爸爸" }, "Home");
        }

    }
}
