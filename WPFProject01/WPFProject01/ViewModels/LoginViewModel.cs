using System.Configuration;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WPFProject01.Entities;
using WPFProject01.enums;
using WPFProject01.Messages;
using WPFProject01.Repository;
using WPFProject01.Service;
using WPFProject01.Views;

namespace WPFProject01.ViewModels;

public partial class LoginViewModel :ObservableObject{
	private readonly IUserService _userService;
	private readonly INavigationService _navigationService;
	private readonly UserSession _userSession;

	public LoginViewModel(IUserService userService,INavigationService navigationService,UserSession userSession) {
		_userService = userService;
		_navigationService = navigationService;
		_userSession = userSession;
		// username = _userSession.User.Username;
		// password = _userSession.User.Password;
	}	
	[ObservableProperty]
	private string username="admin" ;
	[ObservableProperty]
	private string password = "admin";
	
	[RelayCommand]
	async void Login() {
		var user = await _userService.Login(Username, Password);
		if (user!=null) {
			WeakReferenceMessenger.Default.Send(new DialogMessage("登录成功"));
			_userSession.User = user;
			var windowResizeMessage = new WindowResizeMessage(1000,600);
			WeakReferenceMessenger.Default.Send(windowResizeMessage);
			WeakReferenceMessenger.Default.Send(new LoginMessage(true));
			_navigationService.NavigationTo<MainView>();
		} else {
			WeakReferenceMessenger.Default.Send(new DialogMessage("登录失败,用户名或者密码错误"){Buttons = DialogButtons.OK});
		}
			
	}

	[RelayCommand]
	async void Register() {
		_navigationService.NavigationTo<RegisterUserView>();
	}
}