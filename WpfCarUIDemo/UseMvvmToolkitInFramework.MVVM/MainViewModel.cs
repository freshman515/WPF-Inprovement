using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace UseMvvmToolkitInFramework.MVVM;

public partial class MainViewModel :ObservableObject {
    [ObservableProperty] private string message = "hello,world";

    [RelayCommand]
    void Click() { 
        Message = "Clicked!";
    }
}