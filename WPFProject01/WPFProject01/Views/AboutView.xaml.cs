using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.ViewModels;

namespace WPFProject01.Views;

public partial class AboutView : UserControl {
	public AboutView() {
		InitializeComponent();
		this.DataContext = App.Service.GetService<AboutViewModel>();
	}
	private void OnConnect(object sender, RoutedEventArgs e) {
		StatusIndicator.State = "Connected";
	}

	private void OnDisconnect(object sender, RoutedEventArgs e) {
		StatusIndicator.State = "Disconnected";
	}
}