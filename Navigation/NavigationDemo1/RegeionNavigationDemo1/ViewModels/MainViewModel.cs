using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RegeionNavigationDemo1.Extension;
using RegeionNavigationDemo1.Interfaces;
using RegeionNavigationDemo1.Models;
using RegeionNavigationDemo1.Views;
using System.Windows;

namespace RegeionNavigationDemo1.ViewModels {
    public partial class MainViewModel : ObservableObject {
        [ObservableProperty] private FrameworkElement? _currentView;
        [ObservableProperty] private FrameworkElement? _currentView2;
        [ObservableProperty] private int userId;
        [ObservableProperty] private string? _name;
        private int count = 0;
        private readonly IRegionNavigationService _nav;
        private readonly IDialogSerivce _dialogService;
        private readonly IToastService _toastService;
        private readonly ILoggingService _loggingService;

        public MainViewModel(IRegionNavigationService navigationService,IDialogSerivce dialogService,IToastService toastService,ILoggingService loggingService) {
            _nav = navigationService;
            _dialogService = dialogService;
            _toastService = toastService;
            _loggingService = loggingService;
            _nav.RegisterRegion("MainRegion", v => CurrentView = v);
            _nav.RegisterRegion("OtherRegion", v => CurrentView2 = v);
        }

        [RelayCommand]
        private void ShowNewWindiow() {
            var view = App.Services.CreateView<ShellView>();
            view.Show();
        }
        [RelayCommand]
        private void GoHome() {
            _nav.Go<HomeView, UserDetailArgs>("MainRegion", new UserDetailArgs() { UserId = UserId, UserName = Name });
            _toastService.ShowSuccess("回到HomeView");
            _loggingService.Debug("回到HomeView");
            _loggingService.Warn("回到HomeView");
            _loggingService.Info("回到HomeView");
            _loggingService.Error("回到HomeView");
        }
        [RelayCommand]
        private void GoEdit() {
            _nav.Go<EditView>("MainRegion");
            _toastService.ShowHint("导航到EditView");
            _loggingService.Debug("导航到EditView");
            _loggingService.Warn("导航到EditView");
            _loggingService.Info("导航到EditView");
            _loggingService.Error("导航到EditView");

        }
        [RelayCommand]
        private void GoSettings() {
            _nav.Go<SettingsView, string>("MainRegion", $"helloworld{count++}");
            _toastService.ShowError("导航到SettingsView");
            _loggingService.Debug("导航到SettingsView");
            _loggingService.Warn("导航到SettingsView");
            _loggingService.Info("导航到SettingsView");
            _loggingService.Error("导航到SettingsView");

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

        [RelayCommand]
        private async void DeleteItemAsync() {
            var result = await _dialogService.ShowDialogAsync<ConfirmDialog, string, bool>("确定要删除吗");
            _dialogService.ShowMessageBox(result?"删除成功":"删除失败");
        }
    }
}
