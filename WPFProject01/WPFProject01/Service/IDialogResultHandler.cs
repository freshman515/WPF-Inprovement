namespace WPFProject01.Service;

public interface IDialogResultHandler<T> {
	void SetDialogCloseAction(Action<T?> closeAction);
}