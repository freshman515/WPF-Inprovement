using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoApp3Framework.Models;

[ObservableObject]
public partial class Book {
    [ObservableProperty] private string name;
}