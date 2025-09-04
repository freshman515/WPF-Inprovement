using System.Configuration;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfApp2Binding;
[ObservableObject]
public partial class MainViewModel {
    public static int Answer { get; }= 42;
    [ObservableProperty] private string firstName="john";
    [ObservableProperty] private string lastName="doe";

    [RelayCommand]
    void CloseWindow(Window window) {
	    SystemCommands.CloseWindow(window);
    }
}