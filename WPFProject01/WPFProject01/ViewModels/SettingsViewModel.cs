using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using WPFProject01.Helper;

namespace WPFProject01.ViewModels;

public partial class SettingsViewModel:ObservableObject {
	public SettingsViewModel() {
		ShowMessage("helloworld");
	}
	private readonly PaletteHelper _paletteHelper = new();

	[ObservableProperty]
	private bool isDarkTheme;

	partial void OnIsDarkThemeChanged(bool value)
	{
		var theme = _paletteHelper.GetTheme();
		theme.SetBaseTheme(value ? BaseTheme.Dark : BaseTheme.Light);		
		_paletteHelper.SetTheme(theme);
	}	
	public SnackbarMessageQueue SnackbarQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

	[RelayCommand]
	void Click() {
		ShowMessage("helloworld");	
	}
	public void ShowMessage(string message)
	{
		// 确保操作在UI线程上进行
		GlobalSnackbar.Show(message);
	}
}