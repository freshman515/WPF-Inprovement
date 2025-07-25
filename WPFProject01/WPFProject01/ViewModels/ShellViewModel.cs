using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using WPFProject01.Messages;

namespace WPFProject01.ViewModels;

public partial class ShellViewModel : ObservableObject {
	[ObservableProperty] private object currentPage;
	[ObservableProperty] private bool isVisibility;
	[ObservableProperty] private bool _toggleButtonIsVisibility;
	public ShellViewModel() {
		WeakReferenceMessenger.Default.Register<LoginMessage>(this, (r, m) => {
			ToggleButtonIsVisibility = m.Value;
		});
		WeakReferenceMessenger.Default.Register<ToggleButtonVisibilityMessage>(this, (r, m) => {
			if (IsVisibility != m.Value) {
				IsVisibility = m.Value;
			}
		});	
	}

	public static SnackbarMessageQueue SnackbarQueueInstance { get; } = new(TimeSpan.FromSeconds(1));
	public SnackbarMessageQueue GlobalSnackbarQueue => SnackbarQueueInstance;

	[RelayCommand]
	void Mini(object? parameter) {
		if (parameter is Window window) {
			window.WindowState = WindowState.Minimized;
		}
	}

	[RelayCommand]
	void ToggleNav() {
		Console.WriteLine(IsVisibility);
		WeakReferenceMessenger.Default.Send<NavVisibilityMessage>(new NavVisibilityMessage(IsVisibility));
	}

	[RelayCommand]
	void Maxi(object? parameter) {
		if (parameter is Window window) {
			window.WindowState = window.WindowState == WindowState.Normal
				? WindowState.Maximized
				: WindowState.Normal;
		}
	}

	[RelayCommand]
	void Close(object? parameter) {
		if (parameter is Window window) {
			window.Close();
		}
	}
}