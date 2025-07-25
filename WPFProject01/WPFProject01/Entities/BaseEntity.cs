using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFProject01.Entities;

public class BaseEntity:ObservableObject {
	private int id;

	public int Id {
		get => id;
		set => SetProperty(ref id, value);
	}
	
}