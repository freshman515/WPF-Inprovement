using CommunityToolkit.Mvvm.ComponentModel;
using NavigationDemo1.Interfaces;
using NavigationDemo1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationDemo1.ViewModels {
    public partial class SettingsViewModel : ObservableObject, INavigationReceiver<string> {
        [ObservableProperty] private string message;

        public void OnNavigated(string parameter) {
            message = parameter;
        }
    }
}
