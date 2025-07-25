using System.ComponentModel;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.ViewModels;

namespace WPFProject01.Views;

public partial class LoginView : UserControl {
	public LoginView() {
		InitializeComponent();
		InitData();
	}

	private void InitData() {
		this.DataContext = App.Service.GetService<LoginViewModel>();
	}
}