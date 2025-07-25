using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFProject01.Entities;

public class Menu : BaseEntity {
	private string name;

	public string Name {
		get => name;
		set => SetProperty(ref name, value);
	}

	private string icon;

	public string Icon {
		get => icon;
		set => SetProperty(ref icon, value);
	}

	private string? view;

	public string? View {
		get => view;
		set => SetProperty(ref view, value);
	}

	public int? ParentId {
		get => parentId;
		set => SetProperty(ref parentId, value);
	}

	private int? parentId;

	private ObservableCollection<Menu> children = new();

	public ObservableCollection<Menu> Children {
		get => children;
		set => SetProperty(ref children, value);
	}
}