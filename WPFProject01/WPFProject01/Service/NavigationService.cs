using Microsoft.Extensions.DependencyInjection;
using WPFProject01.ViewModels;
using WPFProject01.Views;

namespace WPFProject01.Service;

public class NavigationService : INavigationService {
	private readonly IServiceProvider _serviceProvider;
	private readonly ShellViewModel _shellViewModel;

	public NavigationService(IServiceProvider serviceProvider, ShellViewModel shellViewModel) {
		_serviceProvider = serviceProvider;
		_shellViewModel = shellViewModel;
	}

	public void NavigationTo<T>() where T : class {
		var page = _serviceProvider.GetRequiredService<T>();
		_shellViewModel.CurrentPage = page;	
	}
}