namespace WPFProject01.Service;

public interface INavigationService {
	void NavigationTo<T>() where T : class;
}