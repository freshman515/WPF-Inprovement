using System.Collections.ObjectModel;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WPFProject01.Entities;
using WPFProject01.Messages;
using WPFProject01.Repository;
using WPFProject01.Service;
using WPFProject01.Views;
using Menu = WPFProject01.Entities.Menu;

namespace WPFProject01.ViewModels;

public partial class MainViewModel : ObservableObject {
	private readonly IMenuRepository _menuRepository;
	private readonly INavigationService _navigationService;
	public UserSession UserSessionProp { get; }
	[ObservableProperty] private ObservableCollection<Menu> _menus = new();
	[ObservableProperty] private bool _isNavVisible;
	[ObservableProperty] private object _currentPage;

	public MainViewModel(IMenuRepository menuRepository, UserSession userSession,INavigationService navigationService) {
		_menuRepository = menuRepository;
		_navigationService = navigationService;
		UserSessionProp = userSession;
		RegisterShellViewMessage();	
		_ = InitMenus();
		Navigation("HomeView");
	}

	partial void OnIsNavVisibleChanged(bool oldValue, bool newValue) {
		if (!newValue) {
			//如果抽屉关闭
			WeakReferenceMessenger.Default.Send(new ToggleButtonVisibilityMessage(newValue));
		}
	}

	private void RegisterShellViewMessage() {
		WeakReferenceMessenger.Default.Register<NavVisibilityMessage>(this, (r, m) => {
			IsNavVisible = m.Value;
		});
	}

	[RelayCommand]
	void Logout() {
		_navigationService.NavigationTo<LoginView>();
		WeakReferenceMessenger.Default.Send(new LoginMessage(false));
	}

	[RelayCommand]
	void Navigation(string pageName) {
		var type = Type.GetType($"WPFProject01.Views.{pageName}");
		if (type is not null && typeof(UserControl).IsAssignableFrom(type)) {
			var page = App.Service.GetService(type);
			if (page is UserControl UserControlPage) {
				CurrentPage = UserControlPage;
			}
		}
	}

	private async Task InitMenus() {
		Menus = await _menuRepository.GetMenuItemsAsync();
	}
}