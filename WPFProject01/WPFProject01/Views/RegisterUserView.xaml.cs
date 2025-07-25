using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.ViewModels;

namespace WPFProject01.Views;

public partial class RegisterUserView : UserControl {
	public RegisterUserView() {
		InitializeComponent();
		InitDataContext();
	}

	private void InitDataContext() {
		this.DataContext = App.Service.GetService<RegisterUserViewModel>();
	}
}