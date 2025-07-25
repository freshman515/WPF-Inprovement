using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.Entities;
using WPFProject01.ViewModels;

namespace WPFProject01.Views;

public partial class UserView : UserControl {
	public UserView() {
		InitializeComponent();
		this.DataContext = App.Service.GetService<UserViewModel>();
	}

	private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e) {
	
		if (sender is ListViewItem item && item.DataContext is User user) {
			var vm = DataContext as UserViewModel;
			vm?.SelectedCommand.Execute(user.Id);
		}
	}
}