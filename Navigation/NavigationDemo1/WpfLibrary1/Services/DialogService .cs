using System.Windows;
using Common.Core.Extension;
using Common.Core.Interfaces;

namespace Common.Core.Services;

public class DialogService : IDialogSerivce {
    private readonly IServiceProvider _sp;

    public DialogService(IServiceProvider sp) {
        _sp = sp;
    }
    public async Task<TResult?> ShowDialogAsync<TDialog, TParam, TResult>(TParam parameter) where TDialog : Window, new() {
        var dialog = _sp.CreateView<TDialog>(); // 用你已有的扩展方法自动绑 VM
        if (dialog.DataContext is IDialogReceiver<TParam> receiver)
            receiver.OnDialogOpened(parameter);
        // 异步等待窗口关闭
        var tcs = new TaskCompletionSource<TResult?>();
        dialog.Closed += (_, _) => {
            if (dialog.DataContext is IDialogResultProvider<TResult> resultProvider)
                tcs.TrySetResult(resultProvider.GetDialogResult());
            else
                tcs.TrySetResult(default);
        };
        dialog.ShowDialog(); // 模态

        return await tcs.Task;
    }

    public async Task<bool?> ShowDialogAsync<TDialog, TParam>(TParam parameter)
        where TDialog : Window, new() {
        var dialog = _sp.CreateView<TDialog>();

        if (dialog.DataContext is IDialogReceiver<TParam> receiver)
            receiver.OnDialogOpened(parameter);

        var result = dialog.ShowDialog(); // 返回 bool? (OK/Cancel)
        return await Task.FromResult(result);
    }

    public async Task<TResult?> ShowDialogAsync<TDialog, TResult>() where TDialog : Window, new() {

        var dialog = _sp.CreateView<TDialog>();
        var tsc = new TaskCompletionSource<TResult?>();
        dialog.Closed += (_, _) => {
            if (dialog.DataContext is IDialogResultProvider<TResult> provider) {
                tsc.TrySetResult(provider.GetDialogResult());
            } else {
                tsc.TrySetResult(default);
            }
        };
        dialog.ShowDialog();
        return await tsc.Task;
    }

    public async Task<bool?> ShowDialogAsync<TDialog>() where TDialog : Window, new() {
        var dialog = new TDialog();
        return dialog.ShowDialog();
    }

    public void ShowMessageBox(string message, string title = "提示", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information) {
        MessageBox.Show(message, title, buttons, icon);
    }
}