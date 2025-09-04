using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoApp2.Models;

namespace DemoApp2.ViewModels;
[ObservableObject]
public partial class MainViewModel {
    public ObservableCollection<Book> Books { get; set; } = new();
    public MainViewModel() {
        Books.Add(new Book { Name = "Book 1" });
        Books.Add(new Book { Name = "Book 2" });
        Books.Add(new Book { Name = "Book 3" });
        //_ = LoadBooksAsync();
        //LoadBooksCommand.Execute(null);
        Books.Add(new Book { Name = "Book 7" });
        Books.Add(new Book { Name = "Book 8" });
        Books.Add(new Book { Name = "Book 9" });
    }
    [RelayCommand]
    private async Task LoadBooksAsync() {
        await Task.Delay(3000);
        Books.Add(new Book { Name = "Book 4" });
        Books.Add(new Book { Name = "Book 5" });
        Books.Add(new Book { Name = "Book 6" });
    }
}