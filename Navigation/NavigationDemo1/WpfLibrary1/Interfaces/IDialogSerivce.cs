using System.Windows;

namespace Common.Core.Interfaces;

public interface IDialogSerivce {
    Task<TResult?> ShowDialogAsync<TDialog, TParam, TResult>(TParam parameter)
        where TDialog : Window, new();

    Task<bool?> ShowDialogAsync<TDialog, TParam>(TParam parameter)
        where TDialog : Window, new();

    Task<TResult?> ShowDialogAsync<TDialog, TResult>()
        where TDialog : Window, new();
    Task<bool?> ShowDialogAsync<TDialog>()
        where TDialog : Window, new();
    void ShowMessageBox(string message, string title = "提示", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information);
}