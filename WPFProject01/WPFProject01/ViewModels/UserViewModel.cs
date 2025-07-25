using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.DataContracts;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using WPFProject01.Dtos;
using WPFProject01.Entities;
using WPFProject01.enums;
using WPFProject01.Helper;
using WPFProject01.Messages;
using WPFProject01.Service;
using WPFProject01.Views;

namespace WPFProject01.ViewModels;

public partial class UserViewModel : ObservableObject {
	private readonly IUserService _userService;
	private readonly UserSession _userSession;
	private readonly IDialogService _dialogService;

	// public SnackbarMessageQueue SnackbarQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(1));
	[ObservableProperty] ObservableCollection<User> userList = new();
	[ObservableProperty] private string _username;
	[ObservableProperty] private string _telephone;

	public UserViewModel(IUserService userService, UserSession userSession, IDialogService dialogService) {
		_userService = userService;
		_userSession = userSession;
		_dialogService = dialogService;
		UpdateUsers();
	}

	public void ShowMessage(string message) {
		// 确保操作在UI线程上进行
		// Application.Current.Dispatcher.Invoke(() => { SnackbarQueue.Enqueue(message); });
		GlobalSnackbar.Show(message);
	}


	private async Task UpdateUsers() {
		var users = await _userService.GetUsersAsync();
		this.UserList = new ObservableCollection<User>(users);
	}

	[RelayCommand]
	async void AllSelected() {
		await _userService.AllSelected(true);
		ShowMessage("已全选");
	}

	[RelayCommand]
	async void AllUnSelected() {
		await _userService.AllSelected(false);
		ShowMessage("取消全选");
	}

	[RelayCommand]
	async Task Search() {
		if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Telephone)) {
			UpdateUsers();
			return;
		}

		var userQueryDto = new UserQueryDto() { Username = Username, Telephone = Telephone };
		var users = await _userService.GetUsersAsync(userQueryDto);
		UserList = new ObservableCollection<User>(users);
	}

	[RelayCommand]
	async Task Reset() {
		Username = "";
		Telephone = "";
		await UpdateUsers();
		ShowMessage("Reset Success");
	}

	[RelayCommand]
	async Task Add() {
		if (_userSession.User.Username != "admin") {
			WeakReferenceMessenger.Default.Send(new DialogMessage("权限不足"));
			return;
		}

		var user = await _dialogService.ShowDialog<User>(App.Service.GetService<UserOperatorView>(),
			App.Service.GetService<UpdateViewModel>());
		if (user != null) {
			if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password) ||
			    string.IsNullOrWhiteSpace(user.Telephone) || string.IsNullOrWhiteSpace(user.Role.ToString())) {
				WeakReferenceMessenger.Default.Send(new DialogMessage("输入信息不完整"));
				return;
			}

			await _userService.AddUserAsync(user);
			await UpdateUsers();
			ShowMessage($"添加用户{user.Username}成功");
		} else {
			Console.WriteLine("cancel");
		}
	}

	[RelayCommand]
	async Task Edit(User user) {
		if (_userSession.User.Username != "admin") {
			await App.ShowMessageAsync("提示", "权限不足", DialogButtons.OK);
			return;
		}

		var view = App.Service.GetService<UserOperatorView>();
		var viewModel = App.Service.GetService<UpdateViewModel>();
		viewModel.User = user;
		var userRes = await _dialogService.ShowDialog<User>(view, viewModel);
		if (userRes != null) {
			if (string.IsNullOrWhiteSpace(userRes.Username) || string.IsNullOrWhiteSpace(userRes.Password) ||
			    string.IsNullOrWhiteSpace(userRes.Telephone) || string.IsNullOrWhiteSpace(userRes.Role.ToString())) {
				await App.ShowMessageAsync("提示", "修改失败，信息输入不完整", DialogButtons.OK);
				return;
			}

			await _userService.UpdateUserAsync(userRes);
			ShowMessage($"修改用户{userRes.Username}成功");
		}
	}

	[RelayCommand]
	void RecyclingStation() {
	}

	[RelayCommand]
	async Task DeleteAll() {
		if (_userSession.User.Username != "admin") {
			await App.ShowMessageAsync("提示", "权限不足", DialogButtons.OK);
			return;
		}
		var res = await App.ShowMessageAsync("提示", "确定要删除所勾选的用户吗？", DialogButtons.YesNo);
		if (!res) {
			return;
		}

		bool flag = false;
		try {
			foreach (var user in UserList) {
				if (user.IsSelected) {
					await _userService.DeleteUserAsync(user.Id);
					await UpdateUsers();
					flag = true;
				}
			}

			if (flag) {
				ShowMessage("删除成功");
			} else {
				ShowMessage("没有勾选用户");
			}
		} catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}

	[RelayCommand]
	async Task Selected(int id) {
		var user = await _userService.GetUserAsync(id);
		user.IsSelected = !user.IsSelected;
		await _userService.SaveAsync();
	}

	[RelayCommand]
	async Task Delete(int id) {
		if (_userSession.User.Username != "admin") {
			await App.ShowMessageAsync("提示", "权限不足", DialogButtons.OK);
			return;
		}
		var dialogRes = await App.ShowMessageAsync("提示", "确定删除吗？", DialogButtons.YesNo);
		if (!dialogRes) {
			return;
		}

		var res = await _userService.DeleteUserAsync(id);
		if (res) {
			Console.WriteLine("删除成功");
			ShowMessage("删除用户成功");
			await UpdateUsers();
		}
	}

	[RelayCommand]
	async Task Export() {
		var res = await App.ShowMessageAsync("提示", "想要导出用户表吗？", DialogButtons.YesNo);
		if (!res) {
			return;
		}

		var dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users");
		Directory.CreateDirectory(dirPath);
		var filePath = Path.Combine(dirPath, $"users_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx");


		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		using var package = new ExcelPackage();
		var worksheet = package.Workbook.Worksheets.Add("Users");

		//表头
		worksheet.Cells[1, 1].Value = "Id";
		worksheet.Cells[1, 2].Value = "用户名";
		worksheet.Cells[1, 3].Value = "手机号";
		worksheet.Cells[1, 4].Value = "角色";
		for (int i = 0; i < UserList.Count; i++) {
			var user = UserList[i];
			worksheet.Cells[i + 2, 1].Value = user.Id;
			worksheet.Cells[i + 2, 2].Value = user.Username;
			worksheet.Cells[i + 2, 3].Value = user.Telephone;
			worksheet.Cells[i + 2, 4].Value = user.Role.ToString();
		}

		//样式
		worksheet.Cells[1, 1, 1, 3].Style.Font.Bold = true;
		worksheet.Cells.AutoFitColumns();

		await File.WriteAllBytesAsync(filePath, await package.GetAsByteArrayAsync());
		await App.ShowMessageAsync("提示", $"{filePath}", DialogButtons.OK);
	}
}