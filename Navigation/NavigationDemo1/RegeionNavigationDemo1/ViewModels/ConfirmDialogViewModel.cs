using System.Windows;
using Common.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RegeionNavigationDemo1.ViewModels;

public partial class ConfirmDialogViewModel : ObservableObject, IDialogReceiver<string>, IDialogResultProvider<bool> {
    [ObservableProperty] private string? _message;
    private bool _confirmed;
    public void OnDialogOpened(string parameter) {
        Message = parameter;
    }

    public bool GetDialogResult() => _confirmed;

    [RelayCommand]
    void Confirm(Window win) {
        _confirmed = true;
        win.DialogResult = true;
        win.Close();
    }
    [RelayCommand]
    void Cancel(Window win) {
        _confirmed = false;
        win.DialogResult = false;
        win.Close();
    }
}