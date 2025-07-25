using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using SourceChord.FluentWPF;
using WPFProject01.Messages;
using WPFProject01.Service;
using WPFProject01.ViewModels;

namespace WPFProject01.Views;

public partial class ShellView : Window {
	private readonly INavigationService _navigationService;
	private readonly UserSession _userSession;
	private readonly IUserService _userService;

	public ShellView(INavigationService navigationService, UserSession userSession, IUserService userService) {
		_navigationService = navigationService;
		_userSession = userSession;
		_userService = userService;
		InitializeComponent();
		InitDataContext();
		InitLoginView();
		RegisterMessenger();
	}

	private void RegisterMessenger() {
		WeakReferenceMessenger.Default.Register<DialogMessage>(this, async (r, m) => {
			var viewModel = new CustomDialogViewModel((result => {
				MaterialDesignThemes.Wpf.DialogHost.Close("ShellDialog", result);
			})) {
				Title = m.Title,
				Content = m.Content,
				Buttons = m.Buttons,
			};
			var dialog = new CustomDialogView() { DataContext = viewModel };
			var result = await DialogHost.Show(dialog, "ShellDialog");
			m.TaskSource.SetResult(result as bool?);
		});

		//注册重新设置窗口大小的消息
		WeakReferenceMessenger.Default.Register<WindowResizeMessage>(this, (sender, message) => {
			this.Width = message.Width;
			this.Height = message.Height;
			SetWindowLocation();
		});
	}

	private void SetWindowLocation() {
		this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2;
		this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2;
	}

	private void InitDataContext() {
		this.DataContext = App.Service.GetService<ShellViewModel>();
	}

	private async Task InitLoginView() {
		_navigationService.NavigationTo<LoginView>();
		SetWindowLocation();
		Console.WriteLine("world");
	}

	private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
		if (e.ChangedButton == MouseButton.Left)
			this.DragMove();
	}
}