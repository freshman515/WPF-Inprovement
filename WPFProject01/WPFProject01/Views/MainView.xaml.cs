using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.ViewModels;
using SourceChord.FluentWPF;
namespace WPFProject01.Views;

public partial class MainView : UserControl {
	public MainView() {
		InitializeComponent();
		InitDataContext();
	}

	private void InitDataContext() {
		this.DataContext = App.Service.GetService<MainViewModel>();
	}
}