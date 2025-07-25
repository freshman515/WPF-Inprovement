using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFProject01.Models;

public partial class Product :ObservableObject{
	[ObservableProperty]
	private int id;

	[ObservableProperty]
	private string name;

	[ObservableProperty]
	private string description;

	[ObservableProperty]
	private double price;

	[ObservableProperty]
	private int stock;

	[ObservableProperty]
	private string categoryName;
}