using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RegeionNavigationDemo1.Interfaces;

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