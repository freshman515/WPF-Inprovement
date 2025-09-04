using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WpfApp1;

public partial class MainViewModel :ObservableObject {
    [ObservableProperty] private string message = "hello";

    [RelayCommand]
    void Foo(object obj) {
        Console.WriteLine(obj.ToString());
    }
}