using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoApp2.Models;

[ObservableObject]
public partial class Book {
    [ObservableProperty] private string name;
}