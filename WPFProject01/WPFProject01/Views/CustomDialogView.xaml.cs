using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.ViewModels;

namespace WPFProject01.Views;

public partial class CustomDialogView : UserControl {
	public CustomDialogView() {
		InitializeComponent();
		InitDataContext();
	}

	private void InitDataContext() {
		// this.DataContext = App.Service.GetService<CustomDialogViewModel>();
	}
}