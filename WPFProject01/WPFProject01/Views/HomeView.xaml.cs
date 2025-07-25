using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.ViewModels;

namespace WPFProject01.Views;

public partial class HomeView : UserControl {
	public HomeView() {
		InitializeComponent();
		this.DataContext = App.Service.GetService<HomeViewModel>();
	}
}