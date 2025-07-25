using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using WPFUI01.View;

namespace WPFUI01.ViewModel;

public partial class MainWindowViewModel : ObservableObject {
	public ObservableCollection<HamburgerMenuGlyphItem> MenuItems { get; set; }

	[ObservableProperty] private HamburgerMenuItem _selectedMenuItem;

	public MainWindowViewModel() {
		MenuItems = new ObservableCollection<HamburgerMenuGlyphItem> {
			new HamburgerMenuGlyphItem { Glyph = "\uE10F", Label = "首页", Tag = new HomeView() },
			new HamburgerMenuGlyphItem { Glyph = "\uE13D", Label = "设置", Tag = new SettingsView() }
		};

		SelectedMenuItem = MenuItems[0];
	}
}