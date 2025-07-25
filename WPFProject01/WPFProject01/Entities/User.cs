using CommunityToolkit.Mvvm.ComponentModel;
using WPFProject01.enums;

namespace WPFProject01.Entities;

public class User:BaseEntity {
	
	private bool isSelected;

	public bool IsSelected
	{
		get => isSelected;
		set => SetProperty(ref isSelected, value);
	}

	private string username;

	public string Username {
		get => username;
		set => SetProperty(ref username, value);
	}

	private string password;

	public string Password {
		get => password;
		set => SetProperty(ref password, value);
	}

	private string telephone;

	public string Telephone {
		get => telephone;
		set => SetProperty(ref telephone, value);
	}

	
	private Role role;

	public Role Role {
		get => role;
		set => SetProperty(ref role, value);
	}
}