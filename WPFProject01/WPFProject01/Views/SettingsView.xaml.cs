using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.ViewModels;

namespace WPFProject01.Views;

public partial class SettingsView : UserControl {
	public SettingsView() {
		InitializeComponent();
		DataContext = App.Service.GetService<SettingsViewModel>();
	}
}