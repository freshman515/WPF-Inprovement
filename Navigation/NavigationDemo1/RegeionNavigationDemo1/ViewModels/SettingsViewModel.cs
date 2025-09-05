using CommunityToolkit.Mvvm.ComponentModel;
using RegeionNavigationDemo1.Interfaces;

namespace RegeionNavigationDemo1.ViewModels {
    public partial class SettingsViewModel : ObservableObject, INavigationReceiver<string> {
        [ObservableProperty] private string message;

        public void OnNavigated(string parameter) {
            message = parameter;
        }
    }
}
