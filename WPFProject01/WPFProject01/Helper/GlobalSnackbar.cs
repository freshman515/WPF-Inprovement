using System.Windows;
using MaterialDesignThemes.Wpf;

namespace WPFProject01.Helper;

public class GlobalSnackbar {
	public static SnackbarMessageQueue Queue { get; } = new(TimeSpan.FromSeconds(1));

	public static void Show(string message) {
		Application.Current.Dispatcher.Invoke(() => { Queue.Enqueue(message); });
	}
}