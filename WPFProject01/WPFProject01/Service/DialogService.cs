using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace WPFProject01.Service;

public class DialogService : IDialogService {
	public async Task<T?> ShowDialog<T>(UserControl? control, object? viewModel) {
		//准备TaskCompletionSource 接收返回值
		var tsc = new TaskCompletionSource<T>();

		//设置DataContext
		control.DataContext = viewModel;

		//绑定关闭action的委托事件，假设ViewModel中会执行这个Action来返回值
		if (viewModel is IDialogResultHandler<T> resultHandler) {
			resultHandler.SetDialogCloseAction(res => {
				tsc.SetResult(res);
				DialogHost.Close("ShellDialog");
			});
		} else {
			throw new InvalidOperationException("ViewModel 必须实现 IDialogResultHandler<T>");
		}
		await DialogHost.Show(control, "ShellDialog");
		return await tsc.Task;
	}
}