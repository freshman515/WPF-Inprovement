using CommunityToolkit.Mvvm.ComponentModel;
using NavigationDemo1.Interfaces;
using NavigationDemo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationDemo1.ViewModels {
    public partial class EditViewModel : ObservableObject, INavigationReceiver {
        [ObservableProperty] private UserDetailArgs args;
        public void OnNavigated(object parameter) {
            if (parameter is UserDetailArgs param) {
                args = param;
            }
        }
    }
}
