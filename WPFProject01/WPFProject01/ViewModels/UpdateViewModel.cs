using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPFProject01.Entities;
using WPFProject01.enums;
using WPFProject01.Service;

namespace WPFProject01.ViewModels;

public partial class UpdateViewModel : ObservableObject, IDialogResultHandler<User> {
	[ObservableProperty] private User _user = new();

	public UpdateViewModel(User? user = null) {
		if (user != null) {
			User = user;
		}

		foreach (Role role in Enum.GetValues(typeof(Role))) {
			Roles.Add(role);
		}

		User.Role = Roles.First();
	}

	public List<Role> Roles { get; } = [];

	[RelayCommand]
	void Confirm() {
		_closeAction?.Invoke(User);
	}

	[RelayCommand]
	void Cancel() {
		_closeAction?.Invoke(null);
	}

	private Action<User?>? _closeAction;
	public void SetDialogCloseAction(Action<User?> closeAction) => _closeAction = closeAction;
}