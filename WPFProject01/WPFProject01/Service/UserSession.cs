using CommunityToolkit.Mvvm.ComponentModel;
using WPFProject01.Entities;

namespace WPFProject01.Service;

public class UserSession :ObservableObject{
	private User user;

	public User User {
		get => user;
		set => SetProperty(ref user, value);
	}
	
	

}