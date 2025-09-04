using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoApp4Framework.Models;

[ObservableObject]
public partial class Book {
    [ObservableProperty] private string name;
}