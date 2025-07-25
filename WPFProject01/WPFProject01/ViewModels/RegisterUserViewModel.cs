using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WPFProject01.Entities;
using WPFProject01.Messages;
using WPFProject01.Repository;
using WPFProject01.Service;
using WPFProject01.Views;

namespace WPFProject01.ViewModels;

public partial class RegisterUserViewModel : ObservableObject {
	private readonly IUserService _userService;
	private readonly INavigationService _navigationService;

	public RegisterUserViewModel(IUserService userService, INavigationService navigationService) {
		_userService = userService;
		_navigationService = navigationService;
	}

	[ObservableProperty] string username;
	[ObservableProperty] string password;
	[ObservableProperty] string confirmPassword;
	[ObservableProperty] string telephone;

	[RelayCommand]
	async void RegisterUser() {
		var res = await _userService.RegisterUser(username, password, confirmPassword, telephone);
		if (res) {
			//注册成功
			Console.WriteLine("register user created");
			ClearBox();
			WeakReferenceMessenger.Default.Send(new DialogMessage("成功", $"成功注册用户{username}"));
			_navigationService.NavigationTo<LoginView>();
		} else {
			ClearBox();
			Console.WriteLine("register user failed");
			WeakReferenceMessenger.Default.Send(new DialogMessage("失败", "用户名和密码不一致"));
		}
	}

	void ClearBox() {
		username = "";
		password = "";
		confirmPassword = "";
	}

	[RelayCommand]
	void Return() {
		_navigationService.NavigationTo<LoginView>();
	}
}