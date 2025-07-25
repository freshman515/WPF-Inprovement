using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPFProject01.enums;

namespace WPFProject01.ViewModels;

public partial class CustomDialogViewModel : ObservableObject {
	private readonly Action<bool?> _closeCallback;

	public CustomDialogViewModel(Action<bool?> closeCallback) {
		_closeCallback = closeCallback;
	}

	[ObservableProperty] private string _content;
	[ObservableProperty] private string _title;
	[ObservableProperty] private DialogButtons _buttons;

	[RelayCommand]
	void Confirm() {
		_closeCallback?.Invoke(true);
	}

	[RelayCommand]
	void Close() {
		_closeCallback?.Invoke(false);
	}
}