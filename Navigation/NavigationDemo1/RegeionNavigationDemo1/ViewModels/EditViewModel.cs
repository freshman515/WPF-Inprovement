using Common.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using RegeionNavigationDemo1.Models;

namespace RegeionNavigationDemo1.ViewModels {
    public partial class EditViewModel : ObservableObject, INavigationReceiver {
        [ObservableProperty] private UserDetailArgs args;
        public void OnNavigated(object parameter) {
            if (parameter is UserDetailArgs param) {
                args = param;
            }
        }
    }
}
